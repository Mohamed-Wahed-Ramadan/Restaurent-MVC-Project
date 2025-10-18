using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Context;
using Models;

namespace Restaurent.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly AppDpContext _context;

        public FavoriteController()
        {
            _context = new AppDpContext();
        }

        // إضافة/إزالة من المفضلة
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { success = false, message = "Please login first" });
            }

            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MenuProductId == productId);

            if (existingFavorite != null)
            {
                _context.Favorites.Remove(existingFavorite);
                await _context.SaveChangesAsync();
                return Json(new { success = true, isFavorite = false, message = "Removed from favorites" });
            }
            else
            {
                var favorite = new Favorite
                {
                    UserId = userId.Value,
                    MenuProductId = productId,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.Favorites.AddAsync(favorite);
                await _context.SaveChangesAsync();
                return Json(new { success = true, isFavorite = true, message = "Added to favorites" });
            }
        }

        // عرض المفضلة
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var favorites = await _context.Favorites
                .Include(f => f.MenuProduct)
                .ThenInclude(mp => mp.Category)
                .Where(f => f.UserId == userId && !f.MenuProduct.IsDeleted)
                .Select(f => f.MenuProduct)
                .ToListAsync();

            return View(favorites);
        }

        // التحقق إذا كان المنتج في المفضلة
        [HttpGet]
        public async Task<JsonResult> IsFavorite(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { isFavorite = false });
            }

            var isFavorite = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.MenuProductId == productId);

            return Json(new { isFavorite = isFavorite });
        }
    }
}