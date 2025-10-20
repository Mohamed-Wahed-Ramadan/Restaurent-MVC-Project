using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Context;
using Models;
using Restaurent.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Restaurent.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDpContext _context;
        private readonly UserManager<User> _userManager;
        private const string SUPER_ADMIN_EMAIL = "medo03459@gmail.com";

        public AdminController(AppDpContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ========== الدوال المساعدة ==========

        private async Task<bool> IsAdminUser()
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
                return false;

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                return userSession != null &&
                       userSession.ContainsKey("IsAdmin") &&
                       bool.Parse(userSession["IsAdmin"]?.ToString() ?? "false");
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> IsSuperAdmin()
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
                return false;

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession != null && userSession.ContainsKey("Email"))
                {
                    var email = userSession["Email"]?.ToString();
                    return email == SUPER_ADMIN_EMAIL || email == "4dm1n@gmail.com";
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // ========== Dashboard والإحصائيات ==========

        public async Task<IActionResult> Dashboard()
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var viewModel = new AdminDashboardVM
            {
                TotalProducts = await _context.MenuProducts.CountAsync(p => !p.IsDeleted),
                TotalCategories = await _context.Categories.CountAsync(c => !c.IsDeleted),
                TotalOrders = await _context.Orders.CountAsync(),
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending"),
                TotalUsers = await _userManager.Users.CountAsync(),
                TodayRevenue = await _context.Orders
                    .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow)
                    .SumAsync(o => (decimal?)o.Total) ?? 0,
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

        // ========== إدارة الطلبات ==========

        public async Task<IActionResult> ManageOrders()
        {
            if (!await IsAdminUser())
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            if (!await IsAdminUser())
            {
                return Json(new { success = false, message = "Access denied" });
            }

            var validStatuses = new[] { "Pending", "Preparing", "Ready", "Delivered", "Completed", "Cancelled" };
            if (!validStatuses.Contains(status))
            {
                return Json(new { success = false, message = "Invalid status" });
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return Json(new { success = false, message = "Order not found" });
            }

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Order status updated successfully" });
        }

        // ========== تنظيف الطلبات القديمة ==========
        // ========== تنظيف الطلبات القديمة ==========

        [HttpGet]
        public async Task<IActionResult> CleanupOldOrders()
        {
            if (!await IsSuperAdmin())
            {
                TempData["ErrorMessage"] = "Access denied. Super Admin only.";
                return RedirectToAction("Dashboard");
            }

            // تغيير من شهر إلى أسبوع
            var cutoffDate = DateTime.Now.AddDays(-7); // أسبوع بدل شهر
            var oldCompletedOrders = await _context.Orders
                .Where(o => o.Status == "Completed" && o.CreatedAt < cutoffDate)
                .ToListAsync();

            var vm = new
            {
                OldOrdersCount = oldCompletedOrders.Count,
                CutoffDate = cutoffDate
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CleanupOldOrders(bool confirm)
        {
            if (!await IsSuperAdmin())
            {
                TempData["ErrorMessage"] = "Access denied. Super Admin only.";
                return RedirectToAction("Dashboard");
            }

            if (!confirm)
            {
                TempData["ErrorMessage"] = "Cleanup was not confirmed.";
                return RedirectToAction("Dashboard");
            }

            try
            {
                // تغيير من شهر إلى أسبوع
                var cutoffDate = DateTime.Now.AddDays(-7); // أسبوع بدل شهر
                var oldCompletedOrders = await _context.Orders
                    .Where(o => o.Status == "Completed" && o.CreatedAt < cutoffDate)
                    .ToListAsync();

                var ordersCount = oldCompletedOrders.Count;

                if (ordersCount > 0)
                {
                    // حذف الطلبات القديمة المكتملة
                    _context.Orders.RemoveRange(oldCompletedOrders);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Successfully cleaned up {ordersCount} completed orders older than {cutoffDate:MMMM dd, yyyy}.";
                }
                else
                {
                    TempData["InfoMessage"] = "No old completed orders found to clean up.";
                }

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred during cleanup: {ex.Message}";
                return RedirectToAction("Dashboard");
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetOrderDetails(int orderId)
        {
            if (!await IsAdminUser())
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
                createdAt = order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                total = order.Total,
                status = order.Status,
                orderType = order.OrderType,
                location = order.Location,
                timePreparing = order.TimePreparing,
                user = new
                {
                    name = order.User.UserName,
                    email = order.User.Email,
                    phone = order.User.PhoneNumber,
                    birthday = order.User.Birthday.ToString("yyyy-MM-dd"),
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

            return Json(new { success = true, data = orderData });
        }

        // ========== إدارة الخصومات ==========

        public async Task<IActionResult> Discounts()
        {
            if (!await IsAdminUser())
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

        public async Task<IActionResult> CreateDiscount()
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiscount(DiscountVM model)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesForModel(model);
                return View(model);
            }

            if (model.EndDate <= model.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date must be after start date");
                await LoadCategoriesForModel(model);
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

        [HttpGet]
        public async Task<IActionResult> EditDiscount(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Discount ID is required";
                return RedirectToAction("Discounts");
            }

            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                TempData["ErrorMessage"] = "Discount not found";
                return RedirectToAction("Discounts");
            }

            var viewModel = new DiscountVM
            {
                Id = discount.Id,
                Name = discount.Name,
                Description = discount.Description,
                DiscountPercentage = discount.DiscountPercentage,
                IsAgeBased = discount.IsAgeBased,
                MinAge = discount.MinAge,
                MaxAge = discount.MaxAge,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                CategoryId = discount.CategoryId
            };

            await LoadCategoriesForModel(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDiscount(int id, DiscountVM model)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesForModel(model);
                return View(model);
            }

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                TempData["ErrorMessage"] = "Discount not found";
                return RedirectToAction("Discounts");
            }

            discount.Name = model.Name;
            discount.Description = model.Description;
            discount.DiscountPercentage = model.DiscountPercentage;
            discount.IsAgeBased = model.IsAgeBased;
            discount.MinAge = model.MinAge;
            discount.MaxAge = model.MaxAge;
            discount.StartDate = model.StartDate;
            discount.EndDate = model.EndDate;
            discount.CategoryId = model.CategoryId == 0 ? null : model.CategoryId;
            discount.UpdatedAt = DateTime.UtcNow;

            _context.Update(discount);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Discount updated successfully!";
            return RedirectToAction("Discounts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            if (!await IsAdminUser())
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDiscountStatus(int id)
        {
            if (!await IsAdminUser())
            {
                return Json(new { success = false, message = "Access denied" });
            }

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return Json(new { success = false, message = "Discount not found" });
            }

            discount.IsActive = !discount.IsActive;
            discount.UpdatedAt = DateTime.UtcNow;

            _context.Update(discount);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                isActive = discount.IsActive,
                message = $"Discount {(discount.IsActive ? "activated" : "deactivated")} successfully"
            });
        }

        // ========== الدوال المساعدة المساندة ==========

        private async Task LoadCategoriesForModel(DiscountVM model)
        {
            var categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }
    }
}