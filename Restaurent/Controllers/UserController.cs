using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurent.Context;
using Restaurent.Models;

namespace Restaurent.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDpContext _context;
        private const string SUPER_ADMIN_EMAIL = "medo03459@gmail.com";

        public UserController()
        {
            _context = new();
        }

        // GET: User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered");
                    return View(user);
                }

                user.IsAdmin = false;
                user.CreatedAt = DateTime.UtcNow;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                // Store user in session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                TempData["SuccessMessage"] = "Registration successful!";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            return View(user);
        }

        public IActionResult Login()
        {
            return View();
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

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View();
            }

            // Store user in session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

            TempData["SuccessMessage"] = $"Welcome back, {user.Name}!";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Logged out successfully";
            return RedirectToAction("GetAll", "MenuProduct");
        }

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

            ViewBag.UserAge = user.Age;
            return View(user);
        }

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
        public async Task<IActionResult> Edit(User user)
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
                        user.IsAdmin = existingUser.IsAdmin;

                        existingUser.Name = user.Name;
                        existingUser.Email = user.Email;
                        existingUser.Phone = user.Phone;
                        existingUser.Birthday = user.Birthday;
                        existingUser.UpdatedAt = DateTime.UtcNow;

                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();

                        // Update session
                        HttpContext.Session.SetString("UserName", user.Name);
                        HttpContext.Session.SetString("UserEmail", user.Email);

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

        // ==================== ADMIN MANAGEMENT (Super Admin Only) ====================

        public async Task<IActionResult> ManageUsers()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != SUPER_ADMIN_EMAIL)
            {
                TempData["ErrorMessage"] = "Access denied. Super Admin only.";
                return RedirectToAction("Index", "Home");
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