//using Application.Interfaces;
//using AutoMapper;
//using DTOs;
//using Infrastructure.Interfaces;
//using Models;

//namespace Application.Services
//{
//    public class OrderService : IOrderService
//    {
//        private readonly IOrderRepository _orderRepository;
//        private readonly ICartRepository _cartRepository;
//        private readonly IMenuProductRepository _menuProductRepository;
//        private readonly IDiscountRepository _discountRepository;
//        private readonly IUserRepository _userRepository;
//        private readonly IMapper _mapper;

//        public OrderService(
//            IOrderRepository orderRepository,
//            ICartRepository cartRepository,
//            IMenuProductRepository menuProductRepository,
//            IDiscountRepository discountRepository,
//            IUserRepository userRepository,
//            IMapper mapper)
//        {
//            _orderRepository = orderRepository;
//            _cartRepository = cartRepository;
//            _menuProductRepository = menuProductRepository;
//            _discountRepository = discountRepository;
//            _userRepository = userRepository;
//            _mapper = mapper;
//        }

//        public async Task<List<OrderDTO>> GetAllOrdersAsync()
//        {
//            var orders = await _orderRepository.GetAllAsync();
//            return _mapper.Map<List<OrderDTO>>(orders);
//        }

//        public async Task<OrderDTO> GetOrderByIdAsync(int id)
//        {
//            var order = await _orderRepository.GetByIdAsync(id);
//            return _mapper.Map<OrderDTO>(order);
//        }

//        public async Task<OrderDTO> GetOrderByUniqueIdAsync(string uniqueOrderId)
//        {
//            var order = await _orderRepository.GetByUniqueIdAsync(uniqueOrderId);
//            return _mapper.Map<OrderDTO>(order);
//        }

//        public async Task<List<OrderDTO>> GetUserOrdersAsync(string userId)
//        {
//            var orders = await _orderRepository.GetByUserIdAsync(userId);
//            return _mapper.Map<List<OrderDTO>>(orders);
//        }

//        public async Task<List<OrderDTO>> GetRecentOrdersAsync(int count = 10)
//        {
//            var orders = await _orderRepository.GetRecentAsync(count);
//            return _mapper.Map<List<OrderDTO>>(orders);
//        }

//        public async Task<OrderDTO> CreateOrderAsync(OrderCreateDTO orderDto)
//        {
//            var order = _mapper.Map<Order>(orderDto);
//            order.UniqueOrderId = GenerateUniqueOrderId();
//            order.CreatedAt = DateTime.UtcNow;
//            order.Status = "Pending";

//            // Calculate total from order items
//            order.Total = order.OrderItems.Sum(oi => oi.Total);

//            var createdOrder = await _orderRepository.CreateAsync(order);
//            return _mapper.Map<OrderDTO>(createdOrder);
//        }

//        public async Task<OrderDTO> UpdateOrderStatusAsync(int orderId, string status)
//        {
//            var order = await _orderRepository.GetByIdAsync(orderId);
//            if (order == null)
//                throw new ArgumentException("Order not found");

//            order.Status = status;
//            order.UpdatedAt = DateTime.UtcNow;

//            var updatedOrder = await _orderRepository.UpdateAsync(order);
//            return _mapper.Map<OrderDTO>(updatedOrder);
//        }

//        public async Task CancelOrderAsync(int orderId)
//        {
//            var order = await _orderRepository.GetByIdAsync(orderId);
//            if (order == null)
//                throw new ArgumentException("Order not found");

//            if (order.Status != "Pending")
//                throw new InvalidOperationException("Cannot cancel order that is already being processed");

//            // Restore product quantities
//            foreach (var orderItem in order.OrderItems)
//            {
//                var product = await _menuProductRepository.GetByIdAsync(orderItem.MenuProductId);
//                if (product != null)
//                {
//                    product.Quantity += orderItem.Quantity;
//                    product.UpdatedAt = DateTime.UtcNow;
//                    await _menuProductRepository.UpdateAsync(product);
//                }
//            }

//            order.Status = "Cancelled";
//            order.UpdatedAt = DateTime.UtcNow;
//            await _orderRepository.UpdateAsync(order);
//        }

//        public async Task<int> GetPendingOrdersCountAsync()
//        {
//            return await _orderRepository.GetPendingCountAsync();
//        }

//        public async Task<decimal> GetTodayRevenueAsync()
//        {
//            return await _orderRepository.GetTodayRevenueAsync();
//        }

//        public async Task CleanupOldOrdersAsync(int daysOld = 7)
//        {
//            await _orderRepository.CleanupOldOrdersAsync(daysOld);
//        }

//        public async Task<int> CalculateEstimatedTimeAsync(List<CartDTO> cartItems, string orderType)
//        {
//            var productIds = cartItems.Select(c => c.MenuProductId).ToList();
//            var products = await _menuProductRepository.GetAllAsync();
//            var relevantProducts = products.Where(p => productIds.Contains(p.Id)).ToList();

//            var maxPreparationTime = relevantProducts.Any() ? relevantProducts.Max(p => p.MaxTime) : 20;

//            // Add extra time based on order type
//            return orderType == "Delivery" ? maxPreparationTime + 30 : maxPreparationTime + 10;
//        }

//        public async Task<decimal> CalculateDiscountAsync(string userId, List<CartDTO> cartItems)
//        {
//            var user = await _userRepository.GetByIdAsync(userId);
//            if (user == null) return 0;

//            var activeDiscounts = await _discountRepository.GetActiveDiscountsAsync();
//            decimal totalDiscount = 0;

//            foreach (var cartItem in cartItems)
//            {
//                var product = await _menuProductRepository.GetByIdAsync(cartItem.MenuProductId);
//                if (product == null) continue;

//                foreach (var discount in activeDiscounts)
//                {
//                    bool categoryMatch = discount.CategoryId == null || discount.CategoryId == product.CategoryId;
//                    bool ageMatch = !discount.IsAgeBased || (user.Age >= (discount.MinAge ?? 0) && user.Age <= (discount.MaxAge ?? 150));

//                    if (categoryMatch && ageMatch)
//                    {
//                        var itemDiscount = (cartItem.Total * discount.DiscountPercentage) / 100;
//                        totalDiscount += itemDiscount;
//                        break;
//                    }
//                }
//            }

//            return totalDiscount;
//        }

//        private string GenerateUniqueOrderId()
//        {
//            return $"ORD-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
//        }
//    }
//}