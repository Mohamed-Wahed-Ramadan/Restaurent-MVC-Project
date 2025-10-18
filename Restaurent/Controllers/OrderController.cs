using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurent.Context;
using Restaurent.Models;
using Restaurent.ViewModels;

namespace Restaurent.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDpContext _context;

        public OrderController()
        {
            _context = new AppDpContext();
        }

        // صفحة تأكيد الطلب
        public async Task<IActionResult> Checkout()
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

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty";
                return RedirectToAction("Index", "Cart");
            }

            var viewModel = new CheckoutVM
            {
                CartItems = cartItems,
                Total = cartItems.Sum(c => c.Total)
            };

            return View(viewModel);
        }

        // تأكيد الطلب
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutVM model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (!ModelState.IsValid)
            {
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

                model.CartItems = cartItems;
                model.Total = cartItems.Sum(c => c.Total);
                return View("Checkout", model);
            }

            // الحصول على عناصر السلة
            var cartItemsToOrder = await _context.Carts
                .Include(c => c.MenuProduct)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItemsToOrder.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty";
                return RedirectToAction("Index", "Cart");
            }

            // حساب الوقت التقديري
            var maxPreparationTime = cartItemsToOrder.Max(c => c.MenuProduct.MaxTime);
            var estimatedTime = model.OrderType == "Delivery" ? maxPreparationTime + 30 : maxPreparationTime;

            // إنشاء الطلب - التصحيح هنا
            var order = new Order
            {
                UserId = userId.Value,
                Total = cartItemsToOrder.Sum(c => c.Total),
                Status = "Pending",
                TimePreparing = estimatedTime,
                OrderType = model.OrderType,
                Location = model.OrderType == "DineIn" ? $"Table {model.TableNumber}" : model.DeliveryAddress,
                TableNumber = model.TableNumber,
                DeliveryAddress = model.DeliveryAddress,
                // التصحيح: استدعاء الدالة static بشكل صحيح
                UniqueOrderId = GenerateUniqueOrderId(), // أو Models.Order.GenerateUniqueOrderId()
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
            }

            await _context.Orders.AddAsync(order);

            // حذف عناصر السلة
            _context.Carts.RemoveRange(cartItemsToOrder);

            await _context.SaveChangesAsync();
            await UpdateCartCount();

            TempData["SuccessMessage"] = $"Order placed successfully! Your order ID is: {order.UniqueOrderId}";
            return RedirectToAction("OrderDetails", new { id = order.Id });
        }

        // دالة مساعدة لإنشاء ID فريد - يمكن إضافتها هنا في الـ Controller
        private static string GenerateUniqueOrderId()
        {
            return $"ORD-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        // تفاصيل الطلب
        public async Task<IActionResult> OrderDetails(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuProduct)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
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

        // طلبات المستخدم
        public async Task<IActionResult> MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
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

///////////////////////////////////////////////////////////////////////////
///