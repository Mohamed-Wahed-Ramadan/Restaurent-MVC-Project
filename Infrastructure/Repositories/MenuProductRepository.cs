using Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repositories
{
    public class MenuProductRepository : IMenuProductRepository
    {
        private readonly AppDpContext _context;

        public MenuProductRepository(AppDpContext context)
        {
            _context = context;
        }

        public async Task<MenuProduct> GetByIdAsync(int id)
        {
            return await _context.MenuProducts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<List<MenuProduct>> GetAllAsync()
        {
            return await _context.MenuProducts
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<MenuProduct>> GetByCategoryAsync(int categoryId)
        {
            return await _context.MenuProducts
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<MenuProduct>> SearchAsync(string searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.MenuProducts
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p =>
                    p.Name.Contains(searchTerm) ||
                    p.Description.Contains(searchTerm) ||
                    p.Category.Name.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }

            return await query.ToListAsync();
        }

        public async Task<List<MenuProduct>> GetLowStockAsync(int threshold = 10)
        {
            return await _context.MenuProducts
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted && p.Quantity <= threshold)
                .OrderBy(p => p.Quantity)
                .ToListAsync();
        }

        public async Task<MenuProduct> CreateAsync(MenuProduct product)
        {
            await _context.MenuProducts.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<MenuProduct> UpdateAsync(MenuProduct product)
        {
            _context.MenuProducts.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                product.UpdatedAt = DateTime.UtcNow;
                await UpdateAsync(product);
            }
        }

        public async Task<bool> ExistsAsync(string name, int? excludeId = null)
        {
            var query = _context.MenuProducts
                .Where(p => p.Name.ToLower() == name.ToLower() && !p.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task UpdateStockAsync(int productId, int newQuantity)
        {
            var product = await GetByIdAsync(productId);
            if (product != null)
            {
                product.Quantity = newQuantity;
                product.UpdatedAt = DateTime.UtcNow;
                await UpdateAsync(product);
            }
        }
    }
}