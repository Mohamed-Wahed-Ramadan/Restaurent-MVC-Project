using Models;

namespace Infrastructure.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<List<Category>> GetAllAsync();
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(string name, int? excludeId = null);
        Task<bool> HasProductsAsync(int categoryId);
        Task<List<string>> GetCategoryNamesAsync();
    }
}