using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurent.Context;
using Restaurent.Models;
using Restaurent.ViewModels;

namespace Restaurent.Controllers
{
    public class MenuProductController : Controller
    {
        private readonly AppDpContext _context;

        public MenuProductController()
        {
            _context = new();
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
        public async Task<IActionResult> Create(MenuCreVw menuVM)
        {
            if (!ModelState.IsValid)
            {
                var cats = await _context.Categories.ToListAsync();
                menuVM.Categories = new SelectList(cats, "Id", "Name");
                return View(menuVM);
            }

            if (await _context.MenuProducts.AnyAsync(p => p.Name == menuVM.Name))
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
                ImageUrl = menuVM.ImageUrl ?? "./images/default.png",
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

            if (product == null || product.IsDeleted)
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

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MenuCreVw viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = await GetCategoriesList();
                return View(viewModel);
            }

            
                var product = await _context.MenuProducts.FindAsync(id);

                if (product == null || product.IsDeleted)
                {
                    return NotFound();
                }

                if (await _context.MenuProducts.AnyAsync(p => p.Name == viewModel.Name && p.Id != id))
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
                product.ImageUrl = viewModel.ImageUrl ?? product.ImageUrl;
                product.MinTime = viewModel.MinTime;
                product.MaxTime = viewModel.MaxTime;
                product.DayStock = viewModel.DayStock;

                _context.Update(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Product updated successfully!";
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.MenuProducts.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.IsDeleted = true;
            //product.IsDeletedBy = 1; 

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

        private async Task<SelectList> GetCategoriesList()
        {
            var cats = await _context.Categories.ToListAsync();
            return new SelectList(cats, "Id", "Name");
        }
       
    }
}