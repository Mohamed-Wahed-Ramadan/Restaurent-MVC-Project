using DTOs;

namespace Application.Interfaces
{
    public interface ICartService
    {
        Task<List<CartDTO>> GetUserCartAsync(string userId);
        Task<CartDTO> AddToCartAsync(CartCreateDTO cartDto);
        Task<CartDTO> UpdateCartItemAsync(CartUpdateDTO cartDto);
        Task RemoveFromCartAsync(int cartId);
        Task ClearUserCartAsync(string userId);
        Task<int> GetCartCountAsync(string userId);
        Task<decimal> CalculateCartTotalAsync(string userId);
    }
}