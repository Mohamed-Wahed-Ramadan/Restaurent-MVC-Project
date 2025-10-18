using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurent.Context;
using Restaurent.Models;
using Restaurent.ViewModels;

namespace Restaurent.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDpContext _context;

        public AdminController()
        {
            _context = new AppDpContext();
        }

        // لوحة تحكم الأدمن
        public async Task<IActionResult> Dashboard()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != "medo03459@gmail.com" && HttpContext.Session.GetString("IsAdmin") != "True")
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var viewModel = new AdminDashboardVM
            {
                TotalProducts = await _context.MenuProducts.CountAsync(p => !p.IsDeleted),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending"),
                TotalUsers = await _context.Users.CountAsync(),
                TodayRevenue = await _context.Orders
                    .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow)
                    .SumAsync(o => o.Total),
                RecentOrders = await _context.Orders
                    .Include(o => o.User)
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(10)
                    .Select(o => new OrderVM
                    {
                        Id = o.Id,
                        UniqueOrderId = o.UniqueOrderId,
                        CreatedAt = o.CreatedAt,
                        Total = o.Total,
                        Status = o.Status,
                        OrderType = o.OrderType,
                        Location = o.Location
                    })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // إدارة العروض
        public async Task<IActionResult> Discounts()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != "medo03459@gmail.com" && HttpContext.Session.GetString("IsAdmin") != "True")
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var discounts = await _context.Discounts
                .Include(d => d.Category)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            return View(discounts);
        }

        // إنشاء عرض جديد
        public async Task<IActionResult> CreateDiscount()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != "medo03459@gmail.com" && HttpContext.Session.GetString("IsAdmin") != "True")
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var categories = await _context.Categories.ToListAsync();
            var viewModel = new DiscountVM
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(7)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount(DiscountVM model)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != "medo03459@gmail.com" && HttpContext.Session.GetString("IsAdmin") != "True")
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            if (!ModelState.IsValid)
            {
                var categories = await _context.Categories.ToListAsync();
                model.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                return View(model);
            }

            var discount = new Discount
            {
                Name = model.Name,
                Description = model.Description,
                DiscountPercentage = model.DiscountPercentage,
                IsAgeBased = model.IsAgeBased,
                MinAge = model.MinAge,
                MaxAge = model.MaxAge,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                CategoryId = model.CategoryId == 0 ? null : model.CategoryId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Discount created successfully!";
            return RedirectToAction("Discounts");
        }

        // إدارة الطلبات
        public async Task<IActionResult> ManageOrders()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != "medo03459@gmail.com" && HttpContext.Session.GetString("IsAdmin") != "True")
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuProduct)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        // تحديث حالة الطلب
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != "medo03459@gmail.com" && HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return Json(new { success = false, message = "Access denied" });
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return Json(new { success = false, message = "Order not found" });
            }

            order.Status = status;
            //order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Order status updated successfully" });
        }

        // خدمة تنظيف الطلبات القديمة
        public async Task<IActionResult> CleanupOldOrders()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != "medo03459@gmail.com")
            {
                TempData["ErrorMessage"] = "Access denied. Super Admin only.";
                return RedirectToAction("Dashboard");
            }

            var sixHoursAgo = DateTime.UtcNow.AddHours(-6);
            var oldOrders = await _context.Orders
                .Where(o => o.CreatedAt < sixHoursAgo && (o.Status == "Completed" || o.Status == "Cancelled"))
                .ToListAsync();

            var oldOrderItems = await _context.OrderItems
                .Where(oi => oi.Order.CreatedAt < sixHoursAgo && (oi.Order.Status == "Completed" || oi.Order.Status == "Cancelled"))
                .ToListAsync();

            _context.OrderItems.RemoveRange(oldOrderItems);
            _context.Orders.RemoveRange(oldOrders);

            var deletedCount = await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Cleaned up {deletedCount} old orders and their items.";
            return RedirectToAction("Dashboard");
        }

        ///////////////////////////////////
        ///
        // حذف التخفيض
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != "medo03459@gmail.com" && HttpContext.Session.GetString("IsAdmin") != "True")
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("Discounts");
            }

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                TempData["ErrorMessage"] = "Discount not found";
                return RedirectToAction("Discounts");
            }

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Discount deleted successfully!";
            return RedirectToAction("Discounts");
        }
        // الحصول على تفاصيل الطلب (للـ AJAX)
        [HttpGet]
        public async Task<JsonResult> GetOrderDetails(int orderId)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != "medo03459@gmail.com" && HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return Json(new { success = false, message = "Access denied" });
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuProduct)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return Json(new { success = false, message = "Order not found" });
            }

            var orderData = new
            {
                id = order.Id,
                uniqueOrderId = order.UniqueOrderId,
                createdAt = order.CreatedAt,
                total = order.Total,
                status = order.Status,
                orderType = order.OrderType,
                location = order.Location,
                timePreparing = order.TimePreparing,
                user = new
                {
                    name = order.User.Name,
                    email = order.User.Email,
                    phone = order.User.Phone,
                    birthday = order.User.Birthday,
                    imageURL = order.User.ImageURL
                },
                orderItems = order.OrderItems.Select(oi => new
                {
                    quantity = oi.Quantity,
                    unitPrice = oi.UnitPrice,
                    total = oi.Total,
                    menuProduct = new
                    {
                        name = oi.MenuProduct.Name,
                        description = oi.MenuProduct.Description,
                        imageUrl = oi.MenuProduct.ImageUrl
                    }
                }).ToList()
            };

            return Json(orderData);
        }
    }
}