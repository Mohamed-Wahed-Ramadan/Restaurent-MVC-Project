using Models;

namespace Infrastructure.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetByIdAsync(int id);
        Task<List<Cart>> GetByUserIdAsync(string userId);
        Task<Cart> GetByUserAndProductAsync(string userId, int productId);
        Task<Cart> CreateAsync(Cart cart);
        Task<Cart> UpdateAsync(Cart cart);
        Task DeleteAsync(int id);
        Task DeleteUserCartAsync(string userId);
        Task<int> GetCartCountAsync(string userId);
    }
}