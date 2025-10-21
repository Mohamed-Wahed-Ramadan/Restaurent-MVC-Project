using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Context;
using Models;
using System.Text.Json;

namespace Restaurent.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly AppDpContext _context;

        public FavoriteController(AppDpContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int productId)
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                return Json(new
                {
                    success = false,
                    message = "Please login first",
                    redirect = Url.Action("Login", "User")
                });
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please login first",
                        redirect = Url.Action("Login", "User")
                    });
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please login first",
                        redirect = Url.Action("Login", "User")
                    });
                }

                var existingFavorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.MenuProductId == productId);

                if (existingFavorite != null)
                {
                    _context.Favorites.Remove(existingFavorite);
                    await _context.SaveChangesAsync();
                    return Json(new
                    {
                        success = true,
                        isFavorite = false,
                        message = "Removed from favorites"
                    });
                }
                else
                {
                    var favorite = new Favorite
                    {
                        UserId = userId,
                        MenuProductId = productId,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _context.Favorites.AddAsync(favorite);
                    await _context.SaveChangesAsync();
                    return Json(new
                    {
                        success = true,
                        isFavorite = true,
                        message = "Added to favorites"
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    success = false,
                    message = "Please login first",
                    redirect = Url.Action("Login", "User")
                });
            }
        }

        public async Task<IActionResult> Index()
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                return RedirectToAction("Login", "User");
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    return RedirectToAction("Login", "User");
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
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
            catch
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet]
        public async Task<JsonResult> IsFavorite(int productId)
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                return Json(new { isFavorite = false });
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    return Json(new { isFavorite = false });
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { isFavorite = false });
                }

                var isFavorite = await _context.Favorites
                    .AnyAsync(f => f.UserId == userId && f.MenuProductId == productId);

                return Json(new { isFavorite = isFavorite });
            }
            catch
            {
                return Json(new { isFavorite = false });
            }
        }
    }
}