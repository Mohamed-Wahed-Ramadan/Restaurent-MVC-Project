using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Context;
using Models;
using Restaurent.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Restaurent.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDpContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(AppDpContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                return Json(new { success = false, message = "Please login first" });
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                var product = await _context.MenuProducts.FindAsync(productId);
                if (product == null || product.IsDeleted)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                if (product.Quantity < quantity)
                {
                    return Json(new { success = false, message = "Insufficient stock" });
                }

                var existingCartItem = await _context.Carts
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.MenuProductId == productId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                    existingCartItem.Total = existingCartItem.Quantity * product.Price;
                    existingCartItem.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    var cartItem = new Cart
                    {
                        UserId = userId,
                        MenuProductId = productId,
                        Quantity = quantity,
                        Total = quantity * product.Price,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _context.Carts.AddAsync(cartItem);
                }

                await _context.SaveChangesAsync();
                await UpdateCartCount();

                return Json(new { success = true, message = "Product added to cart" });
            }
            catch
            {
                return Json(new { success = false, message = "Please login first" });
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

                var cartItems = await _context.Carts
                    .Include(c => c.MenuProduct)
                    .Where(c => c.UserId == userId)
                    .Select(c => new CartVM
                    {
                        Id = c.Id,
                        MenuProductId = c.MenuProductId,
                        ProductName = c.MenuProduct.Name,
                        ImageUrl = c.MenuProduct.ImageUrl,
                        Price = c.MenuProduct.Price,
                        Quantity = c.Quantity,
                        Total = c.Total
                    })
                    .ToListAsync();

                var viewModel = new CartSummaryVM
                {
                    CartItems = cartItems,
                    SubTotal = cartItems.Sum(c => c.Total),
                    Total = cartItems.Sum(c => c.Total),
                    TotalItems = cartItems.Sum(c => c.Quantity)
                };

                return View(viewModel);
            }
            catch
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartId, int quantity)
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                return Json(new { success = false, message = "Please login first" });
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                var cartItem = await _context.Carts
                    .Include(c => c.MenuProduct)
                    .FirstOrDefaultAsync(c => c.Id == cartId && c.UserId == userId);

                if (cartItem == null)
                {
                    return Json(new { success = false, message = "Cart item not found" });
                }

                if (quantity <= 0)
                {
                    _context.Carts.Remove(cartItem);
                }
                else
                {
                    if (cartItem.MenuProduct.Quantity < quantity)
                    {
                        return Json(new { success = false, message = "Insufficient stock" });
                    }

                    cartItem.Quantity = quantity;
                    cartItem.Total = quantity * cartItem.MenuProduct.Price;
                    cartItem.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                await UpdateCartCount();

                var cartItems = await _context.Carts
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                var subtotal = cartItems.Sum(c => c.Total);
                var totalItems = cartItems.Sum(c => c.Quantity);

                return Json(new
                {
                    success = true,
                    subtotal = subtotal.ToString("C2"),
                    total = subtotal.ToString("C2"),
                    totalItems = totalItems
                });
            }
            catch
            {
                return Json(new { success = false, message = "Please login first" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                return Json(new { success = false, message = "Please login first" });
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                var cartItem = await _context.Carts
                    .FirstOrDefaultAsync(c => c.Id == cartId && c.UserId == userId);

                if (cartItem != null)
                {
                    _context.Carts.Remove(cartItem);
                    await _context.SaveChangesAsync();
                    await UpdateCartCount();
                }

                return Json(new { success = true, message = "Product removed from cart" });
            }
            catch
            {
                return Json(new { success = false, message = "Please login first" });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetCartCount()
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                return Json(new { count = 0 });
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    return Json(new { count = 0 });
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { count = 0 });
                }

                var count = await _context.Carts
                    .Where(c => c.UserId == userId)
                    .SumAsync(c => c.Quantity);

                return Json(new { count = count });
            }
            catch
            {
                return Json(new { count = 0 });
            }
        }

        private async Task UpdateCartCount()
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (!string.IsNullOrEmpty(userSessionJson))
            {
                try
                {
                    var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                    if (userSession != null && userSession.ContainsKey("Id"))
                    {
                        var userId = userSession["Id"]?.ToString();
                        if (!string.IsNullOrEmpty(userId))
                        {
                            var count = await _context.Carts
                                .Where(c => c.UserId == userId)
                                .SumAsync(c => c.Quantity);
                            HttpContext.Session.SetInt32("CartCount", count);
                        }
                    }
                }
                catch
                {
                    // تجاهل الخطأ
                }
            }
        }
    }
}