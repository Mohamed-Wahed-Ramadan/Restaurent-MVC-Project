using Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDpContext _context;

        public CartRepository(AppDpContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetByIdAsync(int id)
        {
            return await _context.Carts
                .Include(c => c.MenuProduct)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Cart>> GetByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.MenuProduct)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Cart> GetByUserAndProductAsync(string userId, int productId)
        {
            return await _context.Carts
                .Include(c => c.MenuProduct)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.MenuProductId == productId);
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task DeleteAsync(int id)
        {
            var cart = await GetByIdAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserCartAsync(string userId)
        {
            var cartItems = await GetByUserIdAsync(userId);
            if (cartItems.Any())
            {
                _context.Carts.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCartCountAsync(string userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Quantity);
        }
    }
}