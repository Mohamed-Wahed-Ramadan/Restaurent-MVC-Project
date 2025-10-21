using Models;

namespace Infrastructure.Interfaces
{
    public interface IMenuProductRepository
    {
        Task<MenuProduct> GetByIdAsync(int id);
        Task<List<MenuProduct>> GetAllAsync();
        Task<List<MenuProduct>> GetByCategoryAsync(int categoryId);
        Task<List<MenuProduct>> SearchAsync(string searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice);
        Task<List<MenuProduct>> GetLowStockAsync(int threshold = 10);
        Task<MenuProduct> CreateAsync(MenuProduct product);
        Task<MenuProduct> UpdateAsync(MenuProduct product);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(string name, int? excludeId = null);
        Task UpdateStockAsync(int productId, int newQuantity);
    }
}