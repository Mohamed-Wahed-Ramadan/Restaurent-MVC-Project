using Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDpContext _context;

        public FavoriteRepository(AppDpContext context)
        {
            _context = context;
        }

        public async Task<Favorite> GetByIdAsync(int id)
        {
            return await _context.Favorites
                .Include(f => f.MenuProduct)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<Favorite>> GetByUserIdAsync(string userId)
        {
            return await _context.Favorites
                .Include(f => f.MenuProduct)
                .ThenInclude(mp => mp.Category)
                .Where(f => f.UserId == userId && !f.MenuProduct.IsDeleted)
                .ToListAsync();
        }

        public async Task<Favorite> GetByUserAndProductAsync(string userId, int productId)
        {
            return await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MenuProductId == productId);
        }

        public async Task<Favorite> CreateAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
            return favorite;
        }

        public async Task DeleteAsync(int id)
        {
            var favorite = await GetByIdAsync(id);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsFavoriteAsync(string userId, int productId)
        {
            return await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.MenuProductId == productId);
        }
    }
}