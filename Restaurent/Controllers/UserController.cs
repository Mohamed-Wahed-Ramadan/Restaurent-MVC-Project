using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurent.Context;
using Restaurent.Models;
using System.Security.Cryptography;
using System.Text;

namespace Restaurent.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDpContext _context;
        private const string SUPER_ADMIN_EMAIL = "medo03459@gmail.com";
        private readonly IWebHostEnvironment _environment;

        public UserController(IWebHostEnvironment environment)
        {
            _context = new AppDpContext();
            _environment = environment;
        }

        // GET: User/Login
        public IActionResult Login()
        {
            return View();
        }

        // GET: User/Register  
        public IActionResult Register()
        {
            return View();
        }

        // GET: User/Details
        public async Task<IActionResult> Details()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Edit
        public async Task<IActionResult> Edit()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required");
                return View();
            }

            var hashedPassword = HashPassword(password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View();
            }

            // تخزين بيانات المستخدم في الجلسة
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

            // حفظ صورة اليوزر في الـ Session
            if (!string.IsNullOrEmpty(user.ImageURL))
            {
                HttpContext.Session.SetString("UserImage", user.ImageURL);
            }

            // تحديث عدد عناصر السلة
            var cartCount = await _context.Carts
                .Where(c => c.UserId == user.Id)
                .SumAsync(c => c.Quantity);
            HttpContext.Session.SetInt32("CartCount", cartCount);

            TempData["SuccessMessage"] = $"Welcome back, {user.Name}!";
            return RedirectToAction("GetAll", "MenuProduct");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered");
                    return View(user);
                }

                // تشفير كلمة المرور
                user.Password = HashPassword(user.Password);
                user.IsAdmin = false;
                user.CreatedAt = DateTime.UtcNow;

                // رفع الصورة
                if (imageFile != null && imageFile.Length > 0)
                {
                    user.ImageURL = await SaveImage(imageFile);
                }

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                // تخزين بيانات المستخدم في الجلسة
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                // حفظ صورة اليوزر في الـ Session
                if (!string.IsNullOrEmpty(user.ImageURL))
                {
                    HttpContext.Session.SetString("UserImage", user.ImageURL);
                }

                TempData["SuccessMessage"] = "Registration successful!";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user, IFormFile? imageFile)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null || userId != user.Id)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(user.Id);
                    if (existingUser != null)
                    {
                        // الحفاظ على البيانات الحالية
                        user.IsAdmin = existingUser.IsAdmin;
                        user.CreatedAt = existingUser.CreatedAt;

                        // إذا لم يتم إدخال كلمة مرور جديدة، احتفظ بالقديمة
                        if (string.IsNullOrEmpty(user.Password))
                        {
                            user.Password = existingUser.Password;
                        }
                        else
                        {
                            user.Password = HashPassword(user.Password);
                        }

                        // رفع الصورة الجديدة
                        if (imageFile != null && imageFile.Length > 0)
                        {
                            user.ImageURL = await SaveImage(imageFile);
                        }
                        else
                        {
                            user.ImageURL = existingUser.ImageURL;
                        }

                        // تحديث البيانات
                        existingUser.Name = user.Name;
                        existingUser.Email = user.Email;
                        existingUser.Password = user.Password;
                        existingUser.Phone = user.Phone;
                        existingUser.Birthday = user.Birthday;
                        existingUser.ImageURL = user.ImageURL;
                        existingUser.UpdatedAt = DateTime.UtcNow;

                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();

                        // تحديث الجلسة
                        HttpContext.Session.SetString("UserName", user.Name);
                        HttpContext.Session.SetString("UserEmail", user.Email);

                        // تحديث صورة اليوزر في الـ Session
                        if (!string.IsNullOrEmpty(user.ImageURL))
                        {
                            HttpContext.Session.SetString("UserImage", user.ImageURL);
                        }
                        else
                        {
                            HttpContext.Session.Remove("UserImage");
                        }

                        TempData["SuccessMessage"] = "Profile updated successfully!";
                        return RedirectToAction(nameof(Details));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }

            return View(user);
        }

        // GET: User/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Logged out successfully";
            return RedirectToAction("GetAll", "MenuProduct");
        }

        // تشفير كلمة المرور
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // دالة لحفظ الصور
        private async Task<string> SaveImage(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "users");
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

            return $"/images/users/{uniqueFileName}";
        }

        // إدارة المستخدمين (للالأدمن فقط)
        public async Task<IActionResult> ManageUsers()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != SUPER_ADMIN_EMAIL)
            {
                TempData["ErrorMessage"] = "Access denied. Super Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(int id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != SUPER_ADMIN_EMAIL)
            {
                TempData["ErrorMessage"] = "Access denied. Super Admin only.";
                return RedirectToAction("ManageUsers");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("ManageUsers");
            }

            if (user.Email == SUPER_ADMIN_EMAIL)
            {
                TempData["ErrorMessage"] = "Cannot change Super Admin status";
                return RedirectToAction("ManageUsers");
            }

            user.IsAdmin = !user.IsAdmin;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"User {(user.IsAdmin ? "promoted to Admin" : "demoted to regular User")} successfully";
            return RedirectToAction("ManageUsers");
        }
    }
}