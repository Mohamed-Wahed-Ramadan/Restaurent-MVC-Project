using Models;

namespace Infrastructure.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<Favorite> GetByIdAsync(int id);
        Task<List<Favorite>> GetByUserIdAsync(string userId);
        Task<Favorite> GetByUserAndProductAsync(string userId, int productId);
        Task<Favorite> CreateAsync(Favorite favorite);
        Task DeleteAsync(int id);
        Task<bool> IsFavoriteAsync(string userId, int productId);
    }
}