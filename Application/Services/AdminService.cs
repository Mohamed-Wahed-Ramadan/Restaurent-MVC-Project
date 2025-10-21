//using Application.Interfaces;
//using DTOs;
//using Infrastructure.Interfaces;

//namespace Application.Services
//{
//    public class AdminService : IAdminService
//    {
//        private readonly IOrderRepository _orderRepository;
//        private readonly IMenuProductRepository _menuProductRepository;
//        private readonly ICategoryRepository _categoryRepository;
//        private readonly IUserRepository _userRepository;

//        public AdminService(
//            IOrderRepository orderRepository,
//            IMenuProductRepository menuProductRepository,
//            ICategoryRepository categoryRepository,
//            IUserRepository userRepository)
//        {
//            _orderRepository = orderRepository;
//            _menuProductRepository = menuProductRepository;
//            _categoryRepository = categoryRepository;
//            _userRepository = userRepository;
//        }

//        public async Task<AdminDashboardDTO> GetDashboardDataAsync()
//        {
//            var dashboardData = new AdminDashboardDTO
//            {
//                TotalProducts = await _menuProductRepository.GetAllAsync().ContinueWith(t => t.Result.Count),
//                TotalCategories = await _categoryRepository.GetAllAsync().ContinueWith(t => t.Result.Count),
//                TotalOrders = await _orderRepository.GetAllAsync().ContinueWith(t => t.Result.Count),
//                PendingOrders = await _orderRepository.GetPendingCountAsync(),
//                TotalUsers = await _userRepository.GetAllAsync().ContinueWith(t => t.Result.Count),
//                TodayRevenue = await _orderRepository.GetTodayRevenueAsync(),
//                RecentOrders = await _orderRepository.GetRecentAsync(10).ContinueWith(t =>
//                    t.Result.Select(o => new OrderDTO
//                    {
//                        Id = o.Id,
//                        UniqueOrderId = o.UniqueOrderId,
//                        CreatedAt = o.CreatedAt,
//                        Total = o.Total,
//                        Status = o.Status,
//                        OrderType = o.OrderType,
//                        Location = o.Location
//                    }).ToList())
//            };

//            return dashboardData;
//        }

//        public async Task<int> CleanupOldOrdersAsync(int daysOld = 7)
//        {
//            var oldOrders = await _orderRepository.GetAllAsync();
//            var cutoffDate = DateTime.Now.AddDays(-daysOld);
//            var ordersToDelete = oldOrders.Where(o => o.Status == "Completed" && o.CreatedAt < cutoffDate).ToList();

//            if (ordersToDelete.Any())
//            {
//                // In a real implementation, you would have a bulk delete method
//                // For now, we'll simulate the count
//                return ordersToDelete.Count;
//            }

//            return 0;
//        }
//    }
//}