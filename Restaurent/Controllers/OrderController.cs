using Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Restaurent.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace Restaurent.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDpContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(AppDpContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Checkout()
        {
            // استخدام النظام الموحد للجلسة
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                TempData["ErrorMessage"] = "Please login first to checkout";
                return RedirectToAction("Login", "User");
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    TempData["ErrorMessage"] = "Please login first to checkout";
                    return RedirectToAction("Login", "User");
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Please login first to checkout";
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

                if (!cartItems.Any())
                {
                    TempData["ErrorMessage"] = "Your cart is empty";
                    return RedirectToAction("Index", "Cart");
                }

                // التحقق من توفر المخزون
                foreach (var cartItem in cartItems)
                {
                    var product = await _context.MenuProducts.FindAsync(cartItem.MenuProductId);
                    if (product == null || product.IsDeleted || product.Quantity < cartItem.Quantity)
                    {
                        TempData["ErrorMessage"] = $"Product {cartItem.ProductName} is not available in the requested quantity";
                        return RedirectToAction("Index", "Cart");
                    }
                }

                var subtotal = cartItems.Sum(c => c.Total);
                var discount = await CalculateDiscount(userId, await _context.Carts
                    .Include(c => c.MenuProduct)
                    .Where(c => c.UserId == userId)
                    .ToListAsync());

                var viewModel = new CheckoutVm
                {
                    CartItems = cartItems,
                    SubTotal = subtotal,
                    Total = Math.Max(subtotal - discount, 0)
                };

                ViewBag.Subtotal = subtotal;
                ViewBag.Discount = discount;
                ViewBag.EstimatedTime = await CalculateEstimatedTime(cartItems, "DineIn");

                return View(viewModel);
            }
            catch
            {
                TempData["ErrorMessage"] = "Please login first to checkout";
                return RedirectToAction("Login", "User");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutVm model)
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                TempData["ErrorMessage"] = "Please login first to place order";
                return RedirectToAction("Login", "User");
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    TempData["ErrorMessage"] = "Please login first to place order";
                    return RedirectToAction("Login", "User");
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Please login first to place order";
                    return RedirectToAction("Login", "User");
                }

                // التحقق من الصحة المبدئي
                bool isValid = true;

                if (string.IsNullOrEmpty(model.OrderType))
                {
                    ModelState.AddModelError("OrderType", "Please select order type");
                    isValid = false;
                }

                if (model.OrderType == "DineIn" && !model.TableNumber.HasValue)
                {
                    ModelState.AddModelError("TableNumber", "Table number is required for dine-in orders");
                    isValid = false;
                }

                if (model.OrderType == "Delivery" && string.IsNullOrEmpty(model.DeliveryAddress))
                {
                    ModelState.AddModelError("DeliveryAddress", "Delivery address is required for delivery orders");
                    isValid = false;
                }

                if (!isValid)
                {
                    // إعادة تحميل بيانات السلة إذا فشل التحقق
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

                    var subtotalCalc = cartItems.Sum(c => c.Total);
                    var discountCalc = await CalculateDiscount(userId, await _context.Carts
                        .Include(c => c.MenuProduct)
                        .Where(c => c.UserId == userId)
                        .ToListAsync());

                    model.CartItems = cartItems;
                    model.SubTotal = subtotalCalc;
                    model.Total = Math.Max(subtotalCalc - discountCalc, 0);

                    ViewBag.Subtotal = subtotalCalc;
                    ViewBag.Discount = discountCalc;
                    ViewBag.EstimatedTime = await CalculateEstimatedTime(cartItems, model.OrderType);

                    return View("Checkout", model);
                }

                var cartItemsToOrder = await _context.Carts
                    .Include(c => c.MenuProduct)
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                if (!cartItemsToOrder.Any())
                {
                    TempData["ErrorMessage"] = "Your cart is empty";
                    return RedirectToAction("Index", "Cart");
                }

                // التحقق النهائي من المخزون قبل إنشاء الطلب
                foreach (var cartItem in cartItemsToOrder)
                {
                    if (cartItem.MenuProduct.Quantity < cartItem.Quantity)
                    {
                        TempData["ErrorMessage"] = $"Product {cartItem.MenuProduct.Name} is not available in the requested quantity";
                        return RedirectToAction("Index", "Cart");
                    }
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var estimatedTime = await CalculateEstimatedTimeForOrder(cartItemsToOrder, model.OrderType);

                    var subtotal = cartItemsToOrder.Sum(c => c.Total);
                    var discount = await CalculateDiscount(userId, cartItemsToOrder);
                    var finalTotal = Math.Max(subtotal - discount, 0);

                    var order = new Order
                    {
                        UserId = userId,
                        Total = finalTotal,
                        Status = "Pending",
                        TimePreparing = estimatedTime,
                        OrderType = model.OrderType,
                        Location = model.OrderType == "DineIn" ? $"Table {model.TableNumber}" : model.DeliveryAddress,
                        TableNumber = model.OrderType == "DineIn" ? model.TableNumber : null,
                        DeliveryAddress = model.OrderType == "Delivery" ? model.DeliveryAddress : null,
                        UniqueOrderId = GenerateUniqueOrderId(),
                        CreatedAt = DateTime.UtcNow
                    };

                    // إضافة عناصر الطلب
                    foreach (var cartItem in cartItemsToOrder)
                    {
                        var orderItem = new OrderItem
                        {
                            MenuProductId = cartItem.MenuProductId,
                            Quantity = cartItem.Quantity,
                            UnitPrice = cartItem.MenuProduct.Price,
                            Total = cartItem.Total,
                            CreatedAt = DateTime.UtcNow
                        };
                        order.OrderItems.Add(orderItem);

                        // تحديث المخزون
                        cartItem.MenuProduct.Quantity -= cartItem.Quantity;
                        cartItem.MenuProduct.UpdatedAt = DateTime.UtcNow;
                    }

                    await _context.Orders.AddAsync(order);
                    await _context.SaveChangesAsync();

                    // حذف عناصر السلة بعد نجاح الطلب
                    _context.Carts.RemoveRange(cartItemsToOrder);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    await UpdateCartCount();

                    TempData["SuccessMessage"] = $"Order placed successfully! Your order ID is: {order.UniqueOrderId}";
                    return RedirectToAction("MyOrders");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["ErrorMessage"] = "An error occurred while placing your order. Please try again.";
                    return RedirectToAction("Checkout");
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Please login first to place order";
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetEstimatedTime(string orderType)
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

                var estimatedTime = await CalculateEstimatedTime(cartItems, orderType);
                var displayTime = orderType == "Delivery" ? $"{estimatedTime + 30} minutes" : $"{estimatedTime + 10} minutes";

                return Json(new { success = true, estimatedTime = displayTime });
            }
            catch
            {
                return Json(new { success = false, message = "Please login first" });
            }
        }

        private static string GenerateUniqueOrderId()
        {
            return $"ORD-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private async Task<int> CalculateEstimatedTime(List<CartVM> cartItems, string orderType)
        {
            var productIds = cartItems.Select(c => c.MenuProductId).ToList();
            var products = await _context.MenuProducts
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            var maxPreparationTime = products.Any() ? products.Max(p => p.MaxTime) : 20;
            return maxPreparationTime;
        }

        private async Task<int> CalculateEstimatedTimeForOrder(List<Cart> cartItems, string orderType)
        {
            var maxPreparationTime = cartItems.Any() ? cartItems.Max(c => c.MenuProduct.MaxTime) : 20;
            return maxPreparationTime;
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                TempData["ErrorMessage"] = "Please login to view order details";
                return RedirectToAction("Login", "User");
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    TempData["ErrorMessage"] = "Please login to view order details";
                    return RedirectToAction("Login", "User");
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Please login to view order details";
                    return RedirectToAction("Login", "User");
                }

                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuProduct)
                    .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found";
                    return RedirectToAction("MyOrders");
                }

                var viewModel = new OrderVM
                {
                    Id = order.Id,
                    UniqueOrderId = order.UniqueOrderId,
                    CreatedAt = order.CreatedAt,
                    Total = order.Total,
                    Status = order.Status,
                    OrderType = order.OrderType,
                    Location = order.Location,
                    EstimatedTime = order.TimePreparing,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemVM
                    {
                        ProductName = oi.MenuProduct.Name,
                        ImageUrl = oi.MenuProduct.ImageUrl,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        Total = oi.Total
                    }).ToList()
                };

                return View(viewModel);
            }
            catch
            {
                TempData["ErrorMessage"] = "Please login to view order details";
                return RedirectToAction("Login", "User");
            }
        }

        public async Task<IActionResult> MyOrders()
        {
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                TempData["ErrorMessage"] = "Please login to view your orders";
                return RedirectToAction("Login", "User");
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    TempData["ErrorMessage"] = "Please login to view your orders";
                    return RedirectToAction("Login", "User");
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Please login to view your orders";
                    return RedirectToAction("Login", "User");
                }

                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuProduct)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new OrderVM
                    {
                        Id = o.Id,
                        UniqueOrderId = o.UniqueOrderId,
                        CreatedAt = o.CreatedAt,
                        Total = o.Total,
                        Status = o.Status,
                        OrderType = o.OrderType,
                        Location = o.Location,
                        EstimatedTime = o.TimePreparing,
                        OrderItems = o.OrderItems.Select(oi => new OrderItemVM
                        {
                            ProductName = oi.MenuProduct.Name,
                            ImageUrl = oi.MenuProduct.ImageUrl,
                            Quantity = oi.Quantity,
                            UnitPrice = oi.UnitPrice,
                            Total = oi.Total
                        }).ToList()
                    })
                    .ToListAsync();

                return View(orders);
            }
            catch
            {
                TempData["ErrorMessage"] = "Please login to view your orders";
                return RedirectToAction("Login", "User");
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

        private async Task<decimal> CalculateDiscount(string userId, List<Cart> cartItems)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return 0;

            var now = DateTime.UtcNow;
            var activeDiscounts = await _context.Discounts
                .Where(d => d.IsActive && d.StartDate <= now && d.EndDate >= now)
                .ToListAsync();

            decimal totalDiscount = 0;

            foreach (var cartItem in cartItems)
            {
                var product = await _context.MenuProducts
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == cartItem.MenuProductId);

                if (product == null) continue;

                foreach (var discount in activeDiscounts)
                {
                    bool categoryMatch = discount.CategoryId == null || discount.CategoryId == product.CategoryId;

                    bool ageMatch = !discount.IsAgeBased ||
                                  (user.Age >= (discount.MinAge ?? 0) && user.Age <= (discount.MaxAge ?? 150));

                    if (categoryMatch && ageMatch)
                    {
                        var itemDiscount = (cartItem.Total * discount.DiscountPercentage) / 100;
                        totalDiscount += itemDiscount;
                        break;
                    }
                }
            }

            return totalDiscount;
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
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

                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuProduct)
                    .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

                if (order == null)
                {
                    return Json(new { success = false, message = "Order not found" });
                }

                if (order.Status != "Pending")
                {
                    return Json(new { success = false, message = "Cannot cancel order that is already being processed" });
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // استعادة المخزون
                    foreach (var orderItem in order.OrderItems)
                    {
                        orderItem.MenuProduct.Quantity += orderItem.Quantity;
                        orderItem.MenuProduct.UpdatedAt = DateTime.UtcNow;
                    }

                    order.Status = "Cancelled";
                    order.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Json(new { success = true, message = "Order cancelled successfully" });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = "Error cancelling order" });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Please login first" });
            }
        }
    }
}