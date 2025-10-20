using Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Restaurent.ViewModels;
using System.Text.Json;

namespace Restaurent.Controllers
{
    public class MenuProductController : Controller
    {
        private readonly AppDpContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;

        public MenuProductController(AppDpContext context, IWebHostEnvironment environment, UserManager<User> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Showprd(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.MenuProducts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (menu == null)
            {
                return NotFound();
            }

            // استخدام النظام الموحد للجلسة
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
                            ViewBag.IsFavorite = await _context.Favorites
                                .AnyAsync(f => f.UserId == userId && f.MenuProductId == id);
                        }
                        else
                        {
                            ViewBag.IsFavorite = false;
                        }
                    }
                    else
                    {
                        ViewBag.IsFavorite = false;
                    }
                }
                catch
                {
                    ViewBag.IsFavorite = false;
                }
            }
            else
            {
                ViewBag.IsFavorite = false;
            }

            return View(menu);
        }

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var menu = await _context.MenuProducts
                    .Include(p => p.Category)
                    .Where(p => !p.IsDeleted)
                    .ToListAsync();

                // استخدام النظام الموحد للجلسة
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
                                var favorites = await _context.Favorites
                                    .Where(f => f.UserId == userId)
                                    .Select(f => f.MenuProductId)
                                    .ToListAsync();

                                ViewBag.Favorites = favorites;
                                ViewBag.UserId = userId;
                                ViewBag.IsAdmin = userSession.ContainsKey("IsAdmin") && bool.Parse(userSession["IsAdmin"]?.ToString() ?? "false");
                            }
                            else
                            {
                                SetDefaultViewBag();
                            }
                        }
                        else
                        {
                            SetDefaultViewBag();
                        }
                    }
                    catch (Exception ex)
                    {
                        // في حالة خطأ في قراءة الجلسة
                        SetDefaultViewBag();
                    }
                }
                else
                {
                    SetDefaultViewBag();
                }

                var activeDiscounts = await _context.Discounts
                    .Where(d => d.IsActive && d.StartDate <= DateTime.UtcNow && d.EndDate >= DateTime.UtcNow)
                    .ToListAsync();

                ViewBag.ActiveDiscounts = activeDiscounts;

                return View(menu);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading menu.";
                return View(new List<MenuProduct>());
            }
        }

        public async Task<IActionResult> Create()
        {
            var cats = await _context.Categories.ToListAsync();
            MenuCreVw CrtMenuVM = new MenuCreVw()
            {
                Categories = new SelectList(cats, "Id", "Name")
            };
            return View(CrtMenuVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuCreVw menuVM, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                var cats = await _context.Categories.ToListAsync();
                menuVM.Categories = new SelectList(cats, "Id", "Name");
                return View(menuVM);
            }

            if (await _context.MenuProducts.AnyAsync(p => p.Name == menuVM.Name && !p.IsDeleted))
            {
                ModelState.AddModelError("Name", "A product with this name already exists.");
                menuVM.Categories = await GetCategoriesList();
                return View(menuVM);
            }

            var menu = new MenuProduct()
            {
                Name = menuVM.Name,
                Price = menuVM.Price,
                Description = menuVM.Description,
                Quantity = menuVM.Quantity,
                CategoryId = menuVM.CategoryId,
                ImageUrl = await SaveProductImage(imageFile) ?? "./images/default.png",
                MinTime = menuVM.MinTime,
                MaxTime = menuVM.MaxTime,
                DayStock = menuVM.DayStock
            };

            await _context.MenuProducts.AddAsync(menu);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product created successfully!";
            return RedirectToAction("GetAll");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.MenuProducts.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new MenuCreVw
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                MinTime = product.MinTime,
                MaxTime = product.MaxTime,
                DayStock = product.DayStock,
                Categories = await GetCategoriesList()
            };

            if (product.IsDeleted)
            {
                TempData["IsDeleted"] = true;
                ViewBag.IsDeleted = true;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuCreVw viewModel, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = await GetCategoriesList();
                return View(viewModel);
            }

            var product = await _context.MenuProducts.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (await _context.MenuProducts.AnyAsync(p => p.Name == viewModel.Name && p.Id != id && !p.IsDeleted))
            {
                ModelState.AddModelError("Name", "A product with this name already exists.");
                viewModel.Categories = await GetCategoriesList();
                return View(viewModel);
            }

            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.Quantity = viewModel.Quantity;
            product.CategoryId = viewModel.CategoryId;

            // رفع الصورة الجديدة إذا تم اختيارها
            if (imageFile != null && imageFile.Length > 0)
            {
                product.ImageUrl = await SaveProductImage(imageFile);
            }
            else if (!string.IsNullOrEmpty(viewModel.ImageUrl))
            {
                product.ImageUrl = viewModel.ImageUrl;
            }

            product.MinTime = viewModel.MinTime;
            product.MaxTime = viewModel.MaxTime;
            product.DayStock = viewModel.DayStock;
            product.UpdatedAt = DateTime.UtcNow;

            if (product.IsDeleted)
            {
                product.IsDeleted = false;
                TempData["SuccessMessage"] = "Product restored and updated successfully!";
            }
            else
            {
                TempData["SuccessMessage"] = "Product updated successfully!";
            }

            _context.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(GetAll));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.MenuProducts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.MenuProducts.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.IsDeleted = true;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Update(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product deleted successfully!";
            return RedirectToAction("GetAll");
        }

        [HttpPost]
        public async Task<IActionResult> PermanentDelete(int id)
        {
            var product = await _context.MenuProducts.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.MenuProducts.Remove(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product permanently deleted!";
            return RedirectToAction("GetAll");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCartFromDetails(int productId, int quantity = 1)
        {
            // استخدام النظام الموحد للجلسة
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                TempData["ErrorMessage"] = "Please login first to add items to cart";
                return RedirectToAction("Login", "User");
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("Id"))
                {
                    TempData["ErrorMessage"] = "Please login first to add items to cart";
                    return RedirectToAction("Login", "User");
                }

                var userId = userSession["Id"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Please login first to add items to cart";
                    return RedirectToAction("Login", "User");
                }

                var product = await _context.MenuProducts.FindAsync(productId);
                if (product == null || product.IsDeleted)
                {
                    TempData["ErrorMessage"] = "Product not found";
                    return RedirectToAction("GetAll");
                }

                if (product.Quantity < quantity)
                {
                    TempData["ErrorMessage"] = "Insufficient stock";
                    return RedirectToAction("Showprd", new { id = productId });
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

                TempData["SuccessMessage"] = "Product added to cart successfully!";
                return RedirectToAction("Showprd", new { id = productId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Please login first to add items to cart";
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.MenuProducts
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) ||
                                        p.Description.Contains(searchTerm) ||
                                        p.Category.Name.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }

            var results = await query.ToListAsync();

            // استخدام النظام الموحد للجلسة
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
                            var favorites = await _context.Favorites
                                .Where(f => f.UserId == userId)
                                .Select(f => f.MenuProductId)
                                .ToListAsync();

                            ViewBag.Favorites = favorites;
                        }
                    }
                }
                catch
                {
                    // تجاهل الخطأ
                }
            }

            return View("GetAll", results);
        }

        public async Task<IActionResult> FeaturedProducts()
        {
            var activeDiscounts = await _context.Discounts
                .Where(d => d.IsActive && d.StartDate <= DateTime.UtcNow && d.EndDate >= DateTime.UtcNow)
                .ToListAsync();

            var featuredProducts = new List<MenuProduct>();

            foreach (var discount in activeDiscounts)
            {
                IQueryable<MenuProduct> query = _context.MenuProducts
                    .Include(p => p.Category)
                    .Where(p => !p.IsDeleted);

                if (discount.CategoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryId == discount.CategoryId);
                }

                var products = await query.ToListAsync();
                featuredProducts.AddRange(products);
            }

            featuredProducts = featuredProducts.Distinct().ToList();

            ViewBag.ActiveDiscounts = activeDiscounts;
            return View("GetAll", featuredProducts);
        }

        public async Task<IActionResult> LowStockProducts()
        {
            // استخدام النظام الموحد للجلسة
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll");
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("IsAdmin") || !bool.Parse(userSession["IsAdmin"]?.ToString() ?? "false"))
                {
                    TempData["ErrorMessage"] = "Access denied. Admin only.";
                    return RedirectToAction("GetAll");
                }

                var lowStockProducts = await _context.MenuProducts
                    .Include(p => p.Category)
                    .Where(p => !p.IsDeleted && p.Quantity <= 10)
                    .OrderBy(p => p.Quantity)
                    .ToListAsync();

                return View(lowStockProducts);
            }
            catch
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetProductDetails(int id)
        {
            var product = await _context.MenuProducts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            return Json(new
            {
                success = true,
                product = new
                {
                    id = product.Id,
                    name = product.Name,
                    price = product.Price,
                    description = product.Description,
                    imageUrl = product.ImageUrl,
                    category = product.Category?.Name,
                    minTime = product.MinTime,
                    maxTime = product.MaxTime,
                    quantity = product.Quantity,
                    dayStock = product.DayStock
                }
            });
        }

        [HttpGet]
        public async Task<JsonResult> CheckStock(int productId, int quantity = 1)
        {
            var product = await _context.MenuProducts.FindAsync(productId);

            if (product == null || product.IsDeleted)
            {
                return Json(new { available = false, message = "Product not found" });
            }

            if (product.Quantity < quantity)
            {
                return Json(new
                {
                    available = false,
                    message = $"Only {product.Quantity} items available in stock",
                    availableStock = product.Quantity
                });
            }

            return Json(new
            {
                available = true,
                message = "Product available",
                availableStock = product.Quantity
            });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStock(int productId, int newQuantity)
        {
            // استخدام النظام الموحد للجلسة
            var userSessionJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userSessionJson))
            {
                return Json(new { success = false, message = "Access denied" });
            }

            try
            {
                var userSession = JsonSerializer.Deserialize<Dictionary<string, object>>(userSessionJson);
                if (userSession == null || !userSession.ContainsKey("IsAdmin") || !bool.Parse(userSession["IsAdmin"]?.ToString() ?? "false"))
                {
                    return Json(new { success = false, message = "Access denied" });
                }

                var product = await _context.MenuProducts.FindAsync(productId);
                if (product == null || product.IsDeleted)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                if (newQuantity < 0)
                {
                    return Json(new { success = false, message = "Quantity cannot be negative" });
                }

                product.Quantity = newQuantity;
                product.UpdatedAt = DateTime.UtcNow;

                _context.Update(product);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Stock updated successfully",
                    newQuantity = newQuantity
                });
            }
            catch
            {
                return Json(new { success = false, message = "Access denied" });
            }
        }

        [HttpGet]
        public async Task<JsonResult> CheckExistingProduct(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new { exists = false });
            }

            var existingProduct = await _context.MenuProducts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

            if (existingProduct != null)
            {
                return Json(new
                {
                    exists = true,
                    isDeleted = existingProduct.IsDeleted,
                    product = new
                    {
                        id = existingProduct.Id,
                        name = existingProduct.Name,
                        price = existingProduct.Price,
                        category = existingProduct.Category?.Name,
                        imageUrl = existingProduct.ImageUrl,
                        deletedAt = existingProduct.UpdatedAt
                    }
                });
            }

            return Json(new { exists = false });
        }

        // ========== الدوال المساعدة ==========

        private async Task<string?> SaveProductImage(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/products/{uniqueFileName}";
        }

        private async Task<SelectList> GetCategoriesList()
        {
            var cats = await _context.Categories.ToListAsync();
            return new SelectList(cats, "Id", "Name");
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

        // دالة مساعدة لتعيين القيم الافتراضية لـ ViewBag
        private void SetDefaultViewBag()
        {
            ViewBag.Favorites = new List<int>();
            ViewBag.UserId = null;
            ViewBag.IsAdmin = false;
        }
    }
}