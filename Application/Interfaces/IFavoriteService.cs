using DTOs;

namespace Application.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<MenuProductDTO>> GetUserFavoritesAsync(string userId);
        Task<bool> ToggleFavoriteAsync(string userId, int productId);
        Task<bool> IsFavoriteAsync(string userId, int productId);
    }
}