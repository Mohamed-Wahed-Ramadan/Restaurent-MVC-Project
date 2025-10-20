using Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Restaurent.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDpContext _context;
        private readonly UserManager<User> _userManager;

        public CategoryController(AppDpContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            try
            {
                if (!string.IsNullOrEmpty(category.Name))
                {
                    category.Name = category.Name.Trim();
                }

                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    ModelState.AddModelError("Name", "Category name is required");
                    return View(category);
                }

                if (category.Name.Length < 2)
                {
                    ModelState.AddModelError("Name", "Category name must be at least 2 characters long");
                    return View(category);
                }

                if (category.Name.Length > 50)
                {
                    ModelState.AddModelError("Name", "Category name must not exceed 50 characters");
                    return View(category);
                }

                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == category.Name.ToLower() && !c.IsDeleted);

                if (existingCategory != null)
                {
                    ModelState.AddModelError("Name", "A category with this name already exists");
                    return View(category);
                }

                category.CreatedAt = DateTime.UtcNow;
                category.IsDeleted = false;

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Category '{category.Name}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the category: " + ex.Message);
                return View(category);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Category ID is required";
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null || category.IsDeleted)
            {
                TempData["ErrorMessage"] = "Category not found";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                TempData["ErrorMessage"] = "Category ID mismatch";
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            try
            {
                if (!string.IsNullOrEmpty(category.Name))
                {
                    category.Name = category.Name.Trim();
                }

                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    ModelState.AddModelError("Name", "Category name is required");
                    return View(category);
                }

                if (category.Name.Length < 2)
                {
                    ModelState.AddModelError("Name", "Category name must be at least 2 characters long");
                    return View(category);
                }

                if (category.Name.Length > 50)
                {
                    ModelState.AddModelError("Name", "Category name must not exceed 50 characters");
                    return View(category);
                }

                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == category.Name.ToLower()
                                           && c.Id != id
                                           && !c.IsDeleted);

                if (existingCategory != null)
                {
                    ModelState.AddModelError("Name", "A category with this name already exists");
                    return View(category);
                }

                var categoryToUpdate = await _context.Categories.FindAsync(id);

                if (categoryToUpdate == null || categoryToUpdate.IsDeleted)
                {
                    TempData["ErrorMessage"] = "Category not found";
                    return RedirectToAction(nameof(Index));
                }

                categoryToUpdate.Name = category.Name;
                categoryToUpdate.UpdateAt = DateTime.UtcNow;

                _context.Categories.Update(categoryToUpdate);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Category '{category.Name}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryExists(category.Id))
                {
                    TempData["ErrorMessage"] = "Category not found";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the category: " + ex.Message);
                return View(category);
            }
        }

        private async Task<bool> CategoryExists(int id)
        {
            return await _context.Categories.AnyAsync(e => e.Id == id && !e.IsDeleted);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    TempData["ErrorMessage"] = "Category not found";
                    return RedirectToAction(nameof(Index));
                }

                var hasProducts = await _context.MenuProducts
                    .AnyAsync(p => p.CategoryId == id && !p.IsDeleted);

                if (hasProducts)
                {
                    TempData["ErrorMessage"] = $"Cannot delete category '{category.Name}' because it has associated products";
                    return RedirectToAction(nameof(Index));
                }

                category.IsDeleted = true;
                category.UpdateAt = DateTime.UtcNow;
                _context.Categories.Update(category);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Category '{category.Name}' deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the category: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetCategoryNames()
        {
            var categoryNames = await _context.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => c.Name)
                .ToListAsync();

            return Json(categoryNames);
        }

        [HttpGet]
        public async Task<JsonResult> CheckCategoryExists(string name, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Json(new { exists = false });
            }

            var query = _context.Categories
                .Where(c => c.Name.ToLower() == name.ToLower() && !c.IsDeleted);

            if (id.HasValue)
            {
                query = query.Where(c => c.Id != id.Value);
            }

            var exists = await query.AnyAsync();

            return Json(new { exists = exists });
        }

        [HttpGet]
        public async Task<JsonResult> GetCategoryProducts(int id)
        {
            var products = await _context.MenuProducts
                .Where(p => p.CategoryId == id && !p.IsDeleted)
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    price = p.Price,
                    imageUrl = p.ImageUrl
                })
                .ToListAsync();

            return Json(new { success = true, products = products });
        }
    }
}