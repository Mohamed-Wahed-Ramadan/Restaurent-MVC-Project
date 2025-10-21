//using Application.Interfaces;
//using AutoMapper;
//using DTOs;
//using Infrastructure.Interfaces;
//using Models;

//namespace Application.Services
//{
//    public class CartService : ICartService
//    {
//        private readonly ICartRepository _cartRepository;
//        private readonly IMenuProductRepository _menuProductRepository;
//        private readonly IMapper _mapper;

//        public CartService(ICartRepository cartRepository, IMenuProductRepository menuProductRepository, IMapper mapper)
//        {
//            _cartRepository = cartRepository;
//            _menuProductRepository = menuProductRepository;
//            _mapper = mapper;
//        }

//        public async Task<List<CartDTO>> GetUserCartAsync(string userId)
//        {
//            var cartItems = await _cartRepository.GetByUserIdAsync(userId);
//            return _mapper.Map<List<CartDTO>>(cartItems);
//        }

//        public async Task<CartDTO> AddToCartAsync(CartCreateDTO cartDto)
//        {
//            var product = await _menuProductRepository.GetByIdAsync(cartDto.MenuProductId);
//            if (product == null || product.IsDeleted)
//                throw new ArgumentException("Product not found");

//            if (product.Quantity < cartDto.Quantity)
//                throw new InvalidOperationException("Insufficient stock");

//            var existingCartItem = await _cartRepository.GetByUserAndProductAsync(cartDto.UserId, cartDto.MenuProductId);

//            if (existingCartItem != null)
//            {
//                existingCartItem.Quantity += cartDto.Quantity;
//                existingCartItem.Total = existingCartItem.Quantity * product.Price;
//                existingCartItem.UpdatedAt = DateTime.UtcNow;

//                var updatedCart = await _cartRepository.UpdateAsync(existingCartItem);
//                return _mapper.Map<CartDTO>(updatedCart);
//            }
//            else
//            {
//                var cartItem = new Cart
//                {
//                    UserId = cartDto.UserId,
//                    MenuProductId = cartDto.MenuProductId,
//                    Quantity = cartDto.Quantity,
//                    Total = cartDto.Quantity * product.Price,
//                    CreatedAt = DateTime.UtcNow
//                };

//                var createdCart = await _cartRepository.CreateAsync(cartItem);
//                return _mapper.Map<CartDTO>(createdCart);
//            }
//        }

//        public async Task<CartDTO> UpdateCartItemAsync(CartUpdateDTO cartDto)
//        {
//            var cartItem = await _cartRepository.GetByIdAsync(cartDto.Id);
//            if (cartItem == null)
//                throw new ArgumentException("Cart item not found");

//            var product = await _menuProductRepository.GetByIdAsync(cartItem.MenuProductId);
//            if (product == null || product.IsDeleted)
//                throw new ArgumentException("Product not found");

//            if (product.Quantity < cartDto.Quantity)
//                throw new InvalidOperationException("Insufficient stock");

//            if (cartDto.Quantity <= 0)
//            {
//                await _cartRepository.DeleteAsync(cartDto.Id);
//                return null;
//            }

//            cartItem.Quantity = cartDto.Quantity;
//            cartItem.Total = cartDto.Quantity * product.Price;
//            cartItem.UpdatedAt = DateTime.UtcNow;

//            var updatedCart = await _cartRepository.UpdateAsync(cartItem);
//            return _mapper.Map<CartDTO>(updatedCart);
//        }

//        public async Task RemoveFromCartAsync(int cartId)
//        {
//            await _cartRepository.DeleteAsync(cartId);
//        }

//        public async Task ClearUserCartAsync(string userId)
//        {
//            await _cartRepository.DeleteUserCartAsync(userId);
//        }

//        public async Task<int> GetCartCountAsync(string userId)
//        {
//            return await _cartRepository.GetCartCountAsync(userId);
//        }

//        public async Task<decimal> CalculateCartTotalAsync(string userId)
//        {
//            var cartItems = await _cartRepository.GetByUserIdAsync(userId);
//            return cartItems.Sum(c => c.Total);
//        }
//    }
//}