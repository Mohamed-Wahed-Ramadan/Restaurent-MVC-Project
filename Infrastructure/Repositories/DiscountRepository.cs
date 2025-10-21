using Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDpContext _context;

        public DiscountRepository(AppDpContext context)
        {
            _context = context;
        }

        public async Task<Discount> GetByIdAsync(int id)
        {
            return await _context.Discounts
                .Include(d => d.Category)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Discount>> GetAllAsync()
        {
            return await _context.Discounts
                .Include(d => d.Category)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Discount>> GetActiveDiscountsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Discounts
                .Include(d => d.Category)
                .Where(d => d.IsActive && d.StartDate <= now && d.EndDate >= now)
                .ToListAsync();
        }

        public async Task<Discount> CreateAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
            return discount;
        }

        public async Task<Discount> UpdateAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
            return discount;
        }

        public async Task DeleteAsync(int id)
        {
            var discount = await GetByIdAsync(id);
            if (discount != null)
            {
                _context.Discounts.Remove(discount);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ToggleStatusAsync(int id, bool isActive)
        {
            var discount = await GetByIdAsync(id);
            if (discount != null)
            {
                discount.IsActive = isActive;
                discount.UpdatedAt = DateTime.UtcNow;
                await UpdateAsync(discount);
            }
        }
    }
}