using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Context;
using Models;

namespace Restaurent.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDpContext _context;

        public CategoryController()
        {
            _context = new AppDpContext();
        }

        // GET: Category
        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(categories);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                // تنظيف الاسم
                if (!string.IsNullOrEmpty(category.Name))
                {
                    category.Name = category.Name.Trim();
                }

                // التحقق من الصحة
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

                // التحقق من التكرار
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == category.Name.ToLower() && !c.IsDeleted);

                if (existingCategory != null)
                {
                    ModelState.AddModelError("Name", "A category with this name already exists");
                    return View(category);
                }

                // إعداد البيانات
                category.CreatedAt = DateTime.UtcNow;
                category.IsDeleted = false;

                // الحفظ
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


        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Category ID is required";
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null || category.IsDeleted)
            {
                TempData["ErrorMessage"] = "Category not found";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                TempData["ErrorMessage"] = "Category ID mismatch";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // تنظيف الاسم
                if (!string.IsNullOrEmpty(category.Name))
                {
                    category.Name = category.Name.Trim();
                }

                // التحقق من الصحة
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

                // التحقق من التكرار
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == category.Name.ToLower()
                                           && c.Id != id
                                           && !c.IsDeleted);

                if (existingCategory != null)
                {
                    ModelState.AddModelError("Name", "A category with this name already exists");
                    return View(category);
                }

                // الحصول على الفئة الحالية
                var categoryToUpdate = await _context.Categories.FindAsync(id);

                if (categoryToUpdate == null || categoryToUpdate.IsDeleted)
                {
                    TempData["ErrorMessage"] = "Category not found";
                    return RedirectToAction(nameof(Index));
                }

                // تحديث البيانات
                categoryToUpdate.Name = category.Name;
                categoryToUpdate.UpdateAt = DateTime.UtcNow;
                // categoryToUpdate.IsUpdateBy = userId; // إذا كان لديك نظام مستخدمين

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
        // POST: Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    TempData["ErrorMessage"] = "Category not found";
                    return RedirectToAction(nameof(Index));
                }

                // التحقق من وجود منتجات مرتبطة بهذه الفئة
                var hasProducts = await _context.MenuProducts
                    .AnyAsync(p => p.CategoryId == id && !p.IsDeleted);

                if (hasProducts)
                {
                    TempData["ErrorMessage"] = $"Cannot delete category '{category.Name}' because it has associated products";
                    return RedirectToAction(nameof(Index));
                }

                // استخدام Soft Delete بدلاً من الحذف الفعلي
                category.IsDeleted = true;
                category.IsDeletedBy = 1; // يمكنك تغيير هذا ليكون ID المستخدم الحالي
                _context.Categories.Update(category);

                // إذا كنت تريد حذف فعلي استخدم:
                // _context.Categories.Remove(category);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Category '{category.Name}' deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the category: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        // API: للحصول على أسماء الفئات (للتحقق من التكرار)
        [HttpGet]
        public async Task<JsonResult> GetCategoryNames()
        {
            var categoryNames = await _context.Categories
                .Select(c => c.Name)
                .ToListAsync();

            return Json(categoryNames);
        }

        // API: للتحقق من وجود فئة معينة
        [HttpGet]
        public async Task<JsonResult> CheckCategoryExists(string name, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Json(new { exists = false });
            }

            var query = _context.Categories
                .Where(c => c.Name.ToLower() == name.ToLower());

            if (id.HasValue)
            {
                query = query.Where(c => c.Id != id.Value);
            }

            var exists = await query.AnyAsync();

            return Json(new { exists = exists });
        }

        // Helper method
        
    }
}