//using Application.Interfaces;
//using AutoMapper;
//using DTOs;
//using Infrastructure.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Models;

//namespace Application.Services
//{
//    public class MenuProductService : IMenuProductService
//    {
//        private readonly IMenuProductRepository _menuProductRepository;
//        private readonly IMapper _mapper;

//        public MenuProductService(IMenuProductRepository menuProductRepository, IMapper mapper)
//        {
//            _menuProductRepository = menuProductRepository;
//            _mapper = mapper;
//        }

//        public async Task<List<MenuProductDTO>> GetAllProductsAsync()
//        {
//            var products = await _menuProductRepository.GetAllAsync();
//            return _mapper.Map<List<MenuProductDTO>>(products);
//        }

//        public async Task<MenuProductDTO> GetProductByIdAsync(int id)
//        {
//            var product = await _menuProductRepository.GetByIdAsync(id);
//            return _mapper.Map<MenuProductDTO>(product);
//        }

//        public async Task<List<MenuProductDTO>> GetProductsByCategoryAsync(int categoryId)
//        {
//            var products = await _menuProductRepository.GetByCategoryAsync(categoryId);
//            return _mapper.Map<List<MenuProductDTO>>(products);
//        }

//        public async Task<List<MenuProductDTO>> SearchProductsAsync(string searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice)
//        {
//            var products = await _menuProductRepository.SearchAsync(searchTerm, categoryId, minPrice, maxPrice);
//            return _mapper.Map<List<MenuProductDTO>>(products);
//        }

//        public async Task<List<MenuProductDTO>> GetLowStockProductsAsync(int threshold = 10)
//        {
//            var products = await _menuProductRepository.GetLowStockAsync(threshold);
//            return _mapper.Map<List<MenuProductDTO>>(products);
//        }

//        public async Task<MenuProductDTO> CreateProductAsync(MenuProductCreateDTO productDto, IFormFile imageFile, string webRootPath)
//        {
//            var product = _mapper.Map<MenuProduct>(productDto);
//            product.CreatedAt = DateTime.UtcNow;
//            product.IsDeleted = false;

//            if (imageFile != null && imageFile.Length > 0)
//            {
//                product.ImageUrl = await SaveImageAsync(imageFile, webRootPath);
//            }
//            else
//            {
//                product.ImageUrl = "/images/default.png";
//            }

//            var createdProduct = await _menuProductRepository.CreateAsync(product);
//            return _mapper.Map<MenuProductDTO>(createdProduct);
//        }

//        public async Task<MenuProductDTO> UpdateProductAsync(MenuProductUpdateDTO productDto, IFormFile imageFile, string webRootPath)
//        {
//            var existingProduct = await _menuProductRepository.GetByIdAsync(productDto.Id);
//            if (existingProduct == null || existingProduct.IsDeleted)
//                throw new ArgumentException("Product not found");

//            existingProduct.Name = productDto.Name;
//            existingProduct.Description = productDto.Description;
//            existingProduct.Price = productDto.Price;
//            existingProduct.Quantity = productDto.Quantity;
//            existingProduct.CategoryId = productDto.CategoryId;
//            existingProduct.MinTime = productDto.MinTime;
//            existingProduct.MaxTime = productDto.MaxTime;
//            existingProduct.DayStock = productDto.DayStock;
//            existingProduct.UpdatedAt = DateTime.UtcNow;

//            if (imageFile != null && imageFile.Length > 0)
//            {
//                existingProduct.ImageUrl = await SaveImageAsync(imageFile, webRootPath);
//            }
//            else if (!string.IsNullOrEmpty(productDto.ImageUrl))
//            {
//                existingProduct.ImageUrl = productDto.ImageUrl;
//            }

//            var updatedProduct = await _menuProductRepository.UpdateAsync(existingProduct);
//            return _mapper.Map<MenuProductDTO>(updatedProduct);
//        }

//        public async Task DeleteProductAsync(int id)
//        {
//            await _menuProductRepository.DeleteAsync(id);
//        }

//        public async Task UpdateProductStockAsync(int productId, int newQuantity)
//        {
//            await _menuProductRepository.UpdateStockAsync(productId, newQuantity);
//        }

//        public async Task<bool> ProductExistsAsync(string name, int? excludeId = null)
//        {
//            return await _menuProductRepository.ExistsAsync(name, excludeId);
//        }

//        public async Task<bool> CheckStockAsync(int productId, int quantity)
//        {
//            var product = await _menuProductRepository.GetByIdAsync(productId);
//            return product != null && product.Quantity >= quantity;
//        }

//        private async Task<string> SaveImageAsync(IFormFile imageFile, string webRootPath)
//        {
//            var uploadsFolder = Path.Combine(webRootPath, "images", "products");
//            if (!Directory.Exists(uploadsFolder))
//            {
//                Directory.CreateDirectory(uploadsFolder);
//            }

//            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
//            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

//            using (var fileStream = new FileStream(filePath, FileMode.Create))
//            {
//                await imageFile.CopyToAsync(fileStream);
//            }

//            return $"/images/products/{uniqueFileName}";
//        }
//    }
//}