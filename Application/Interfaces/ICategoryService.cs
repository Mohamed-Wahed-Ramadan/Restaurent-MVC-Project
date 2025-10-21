using DTOs;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(CategoryCreateDTO categoryDto);
        Task<CategoryDTO> UpdateCategoryAsync(CategoryUpdateDTO categoryDto);
        Task DeleteCategoryAsync(int id);
        Task<bool> CategoryExistsAsync(string name, int? excludeId = null);
        Task<bool> CategoryHasProductsAsync(int id);
        Task<List<string>> GetCategoryNamesAsync();
    }
}