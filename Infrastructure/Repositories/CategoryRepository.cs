using Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDpContext _context;

        public CategoryRepository(AppDpContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                category.IsDeleted = true;
                category.UpdateAt = DateTime.UtcNow;
                await UpdateAsync(category);
            }
        }

        public async Task<bool> ExistsAsync(string name, int? excludeId = null)
        {
            var query = _context.Categories
                .Where(c => c.Name.ToLower() == name.ToLower() && !c.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> HasProductsAsync(int categoryId)
        {
            return await _context.MenuProducts
                .AnyAsync(p => p.CategoryId == categoryId && !p.IsDeleted);
        }

        public async Task<List<string>> GetCategoryNamesAsync()
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => c.Name)
                .ToListAsync();
        }
    }
}