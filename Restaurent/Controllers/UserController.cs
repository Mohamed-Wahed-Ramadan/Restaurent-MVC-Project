using Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Restaurent.ViewModels;
using System.Text.Json;

namespace Restaurent.Controllers
{
    public class UserController : Controller
    {
        private const string SUPER_ADMIN_EMAIL = "medo03459@gmail.com";
        private const string SECOND_SUPER_ADMIN_EMAIL = "4dm1n@gmail.com";
        private const string USER_SESSION_KEY = "CurrentUser";
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;

        public UserController(IWebHostEnvironment environment, UserManager<User> userManager, SignInManager<User> signinManager)
        {
            _environment = environment;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Details(string? id = null)
        {
            var userId = id ?? GetUserIdFromSession();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id = null)
        {
            var userId = id ?? GetUserIdFromSession();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User model, IFormFile? imageFile, string? newPassword)
        {
            ModelState.Remove("PasswordHash");
            ModelState.Remove("SecurityStamp");
            ModelState.Remove("ConcurrencyStamp");
            ModelState.Remove("NormalizedEmail");
            ModelState.Remove("NormalizedUserName");
            ModelState.Remove("imageFile");
            ModelState.Remove("newPassword");

            ModelState.Remove("Orders");
            ModelState.Remove("CartItems");
            ModelState.Remove("Favorites");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUserId = GetUserIdFromSession();
            if (string.IsNullOrEmpty(currentUserId))
            {
                return RedirectToAction("Login");
            }

            try
            {
                var existingUser = await _userManager.FindByIdAsync(model.Id);
                if (existingUser == null)
                {
                    TempData["ErrorMessage"] = "User not found";
                    return RedirectToAction("Details");
                }

                if (existingUser.Id != currentUserId)
                {
                    TempData["ErrorMessage"] = "You can only edit your own profile";
                    return RedirectToAction("Details");
                }

                existingUser.UserName = model.UserName;
                existingUser.Email = model.Email;
                existingUser.PhoneNumber = model.PhoneNumber;
                existingUser.Birthday = model.Birthday;
                existingUser.UpdatedAt = DateTime.UtcNow;

                // تحديث الصورة إذا تم رفع واحدة
                if (imageFile != null && imageFile.Length > 0)
                {
                    existingUser.ImageURL = await SaveImage(imageFile);
                }

                if (!string.IsNullOrEmpty(newPassword))
                {
                    if (newPassword.Length < 6)
                    {
                        ModelState.AddModelError("", "Password must be at least 6 characters");
                        return View(model);
                    }

                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    var result = await _userManager.ResetPasswordAsync(existingUser, token, newPassword);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }

                var updateResult = await _userManager.UpdateAsync(existingUser);
                if (updateResult.Succeeded)
                {
                    await SetUserSession(existingUser);
                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction("Details", new { id = existingUser.Id });
                }
                else
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the profile: " + ex.Message);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UseRememberMe");

            await _signinManager.SignOutAsync();
            ClearUserSession();
            TempData["SuccessMessage"] = "Logged out successfully";
            return RedirectToAction("GetAll", "MenuProduct");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(vm);
                }

                bool isAdmin = user.IsAdmin;

                // المنطق الصحيح:
                // - إذا كان أدمن: isPersistent = false (session cookie فقط)
                // - إذا كان مستخدم عادي واختار RememberMe: isPersistent = true
                // - إذا كان مستخدم عادي ولم يختر RememberMe: isPersistent = false
                bool usePersistentCookie = !isAdmin && vm.RememberMe;

                var res = await _signinManager.PasswordSignInAsync(
                    user,
                    vm.Password,
                    usePersistentCookie,
                    lockoutOnFailure: true
                );

                if (res.Succeeded)
                {
                    await SetUserSession(user);

                    // حفظ تفضيل RememberMe في الـ Session للمستخدمين العاديين فقط
                    if (!isAdmin && vm.RememberMe)
                    {
                        HttpContext.Session.SetString("UseRememberMe", "true");
                    }

                    if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                    {
                        return Redirect(vm.ReturnUrl);
                    }

                    if (user.IsAdmin)
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("GetAll", "MenuProduct");
                    }
                }

                if (res.IsLockedOut)
                {
                    ModelState.AddModelError("", "Account is locked out. Please try again later.");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserVm vm)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(vm.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(vm);
                }

                var user = new User()
                {
                    UserName = vm.FullName,
                    Email = vm.Email,
                    PhoneNumber = vm.PhoneNumber,
                    Birthday = vm.Birthday,
                    CreatedAt = DateTime.UtcNow
                };

                if (vm.ImageFile != null && vm.ImageFile.Length > 0)
                {
                    user.ImageURL = await SaveImage(vm.ImageFile);
                }

                IdentityResult res = await _userManager.CreateAsync(user, vm.Password);

                if (res.Succeeded)
                {
                    // المستخدم الجديد دائماً isPersistent = false عند التسجيل
                    await _signinManager.SignInAsync(user, isPersistent: false);
                    await SetUserSession(user);

                    TempData["SuccessMessage"] = "Registration successful! Welcome to our restaurant.";

                    if (user.IsAdmin)
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("GetAll", "MenuProduct");
                    }
                }

                foreach (var er in res.Errors)
                {
                    ModelState.AddModelError("", er.Description);
                }
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers(string search, string searchBy = "all")
        {
            var currentUserEmail = GetEmailFromSession();
            if (string.IsNullOrEmpty(currentUserEmail) ||
                (currentUserEmail != SUPER_ADMIN_EMAIL && currentUserEmail != SECOND_SUPER_ADMIN_EMAIL))
            {
                TempData["ErrorMessage"] = "Access denied. Super Admin only.";
                return RedirectToAction("GetAll", "MenuProduct");
            }

            var usersQuery = _userManager.Users.AsQueryable();

            if (currentUserEmail == SECOND_SUPER_ADMIN_EMAIL)
            {
                usersQuery = usersQuery.Where(u => u.Email != SUPER_ADMIN_EMAIL);
            }

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();

                switch (searchBy.ToLower())
                {
                    case "name":
                        usersQuery = usersQuery.Where(u =>
                            u.UserName.ToLower().Contains(search) ||
                            u.NormalizedUserName.ToLower().Contains(search));
                        break;
                    case "email":
                        usersQuery = usersQuery.Where(u =>
                            u.Email.ToLower().Contains(search) ||
                            u.NormalizedEmail.ToLower().Contains(search));
                        break;
                    case "phone":
                        usersQuery = usersQuery.Where(u =>
                            u.PhoneNumber != null &&
                            u.PhoneNumber.Contains(search));
                        break;
                    case "all":
                    default:
                        usersQuery = usersQuery.Where(u =>
                            u.UserName.ToLower().Contains(search) ||
                            u.NormalizedUserName.ToLower().Contains(search) ||
                            u.Email.ToLower().Contains(search) ||
                            u.NormalizedEmail.ToLower().Contains(search) ||
                            (u.PhoneNumber != null && u.PhoneNumber.Contains(search)));
                        break;
                }
            }

            var users = await usersQuery.ToListAsync();

            ViewData["SearchTerm"] = search;
            ViewData["SearchBy"] = searchBy;
            ViewData["TotalResults"] = users.Count;

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var currentUserEmail = GetEmailFromSession();
            if (string.IsNullOrEmpty(currentUserEmail) ||
                (currentUserEmail != SUPER_ADMIN_EMAIL && currentUserEmail != SECOND_SUPER_ADMIN_EMAIL))
            {
                TempData["ErrorMessage"] = "Access denied. Super Admin only.";
                return RedirectToAction("ManageUsers");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("ManageUsers");
            }

            if (user.Email == SUPER_ADMIN_EMAIL || user.Email == SECOND_SUPER_ADMIN_EMAIL)
            {
                TempData["ErrorMessage"] = "Cannot change Super Admin status";
                return RedirectToAction("ManageUsers");
            }

            user.IsAdmin = !user.IsAdmin;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User {(user.IsAdmin ? "promoted to Admin" : "demoted to regular User")} successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update user role";
            }

            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentUserEmail = GetEmailFromSession();
            if (string.IsNullOrEmpty(currentUserEmail) ||
                (currentUserEmail != SUPER_ADMIN_EMAIL && currentUserEmail != SECOND_SUPER_ADMIN_EMAIL))
            {
                TempData["ErrorMessage"] = "Access denied. Super Admin only.";
                return RedirectToAction("ManageUsers");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("ManageUsers");
            }

            if (user.Email == SUPER_ADMIN_EMAIL || user.Email == SECOND_SUPER_ADMIN_EMAIL)
            {
                TempData["ErrorMessage"] = "Cannot delete Super Admin";
                return RedirectToAction("ManageUsers");
            }

            var currentUserId = GetUserIdFromSession();
            if (user.Id == currentUserId)
            {
                TempData["ErrorMessage"] = "You cannot delete your own account";
                return RedirectToAction("ManageUsers");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete user";
            }

            return RedirectToAction("ManageUsers");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // ========== الدوال المساعدة ==========

        private string GetUserIdFromSession()
        {
            var userJson = HttpContext.Session.GetString(USER_SESSION_KEY);
            if (string.IsNullOrEmpty(userJson))
                return null;

            try
            {
                var userSession = JsonSerializer.Deserialize<JsonElement>(userJson);
                if (userSession.ValueKind == JsonValueKind.Object)
                {
                    if (userSession.TryGetProperty("id", out var idElement) ||
                        userSession.TryGetProperty("Id", out idElement))
                    {
                        return idElement.GetString();
                    }
                }
            }
            catch
            {
                // تجاهل الأخطاء
            }

            return null;
        }

        private string GetEmailFromSession()
        {
            var userJson = HttpContext.Session.GetString(USER_SESSION_KEY);
            if (string.IsNullOrEmpty(userJson))
                return null;

            try
            {
                var userSession = JsonSerializer.Deserialize<JsonElement>(userJson);
                if (userSession.ValueKind == JsonValueKind.Object)
                {
                    if (userSession.TryGetProperty("email", out var emailElement) ||
                        userSession.TryGetProperty("Email", out emailElement))
                    {
                        return emailElement.GetString();
                    }
                }
            }
            catch
            {
                // تجاهل الأخطاء
            }

            return null;
        }

        private bool IsSuperAdmin()
        {
            var email = GetEmailFromSession();
            return email == SUPER_ADMIN_EMAIL || email == SECOND_SUPER_ADMIN_EMAIL;
        }

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

        private async Task SetUserSession(User user)
        {
            var userSession = new
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Birthday = user.Birthday,
                ImageURL = user.ImageURL,
                IsAdmin = user.IsAdmin,
                Age = user.Age
            };

            var userJson = JsonSerializer.Serialize(userSession);
            HttpContext.Session.SetString(USER_SESSION_KEY, userJson);
            HttpContext.Session.SetString("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
        }

        private void ClearUserSession()
        {
            HttpContext.Session.Remove(USER_SESSION_KEY);
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("IsAdmin");
            HttpContext.Session.Remove("CartCount");
        }
    }
}