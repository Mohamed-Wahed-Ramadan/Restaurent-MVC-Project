using DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IMenuProductService
    {
        Task<List<MenuProductDTO>> GetAllProductsAsync();
        Task<MenuProductDTO> GetProductByIdAsync(int id);
        Task<List<MenuProductDTO>> GetProductsByCategoryAsync(int categoryId);
        Task<List<MenuProductDTO>> SearchProductsAsync(string searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice);
        Task<List<MenuProductDTO>> GetLowStockProductsAsync(int threshold = 10);
        Task<MenuProductDTO> CreateProductAsync(MenuProductCreateDTO productDto, IFormFile imageFile, string webRootPath);
        Task<MenuProductDTO> UpdateProductAsync(MenuProductUpdateDTO productDto, IFormFile imageFile, string webRootPath);
        Task DeleteProductAsync(int id);
        Task UpdateProductStockAsync(int productId, int newQuantity);
        Task<bool> ProductExistsAsync(string name, int? excludeId = null);
        Task<bool> CheckStockAsync(int productId, int quantity);
    }
}