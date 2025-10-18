using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurent.Context;
using Restaurent.Models;
using Restaurent.ViewModels;

namespace Restaurent.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDpContext _context;

        public CartController()
        {
            _context = new AppDpContext();
        }

        // إضافة منتج إلى السلة
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
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
                    UserId = userId.Value,
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

        // عرض السلة
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
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

        // تحديث كمية المنتج في السلة
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
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

        // حذف منتج من السلة
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
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

        // الحصول على عدد العناصر في السلة
        [HttpGet]
        public async Task<JsonResult> GetCartCount()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { count = 0 });
            }

            var count = await _context.Carts
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Quantity);

            return Json(new { count = count });
        }

        private async Task UpdateCartCount()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var count = await _context.Carts
                    .Where(c => c.UserId == userId)
                    .SumAsync(c => c.Quantity);
                HttpContext.Session.SetInt32("CartCount", count);
            }
        }
    }
}