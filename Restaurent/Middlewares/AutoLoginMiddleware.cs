using Microsoft.AspNetCore.Identity;
using Models;

namespace Restaurent.Middlewares
{
    public class AutoLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public AutoLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            // إذا كان المستخدم غير مسجل دخول
            if (!context.User.Identity.IsAuthenticated)
            {
                var userId = context.Session.GetString("UserId");
                var useRememberMe = context.Session.GetString("UseRememberMe");

                // إذا كان هناك مستخدم محفوظ في الجلسة وكان RememberMe مفعل
                if (!string.IsNullOrEmpty(userId) && useRememberMe == "true")
                {
                    var user = await userManager.FindByIdAsync(userId);

                    // تأكد أن المستخدم موجود وليس أدمن
                    if (user != null && !user.IsAdmin)
                    {
                        // سجل دخول المستخدم تلقائياً
                        await signInManager.SignInAsync(user, isPersistent: true);

                        // جدد الـ Session
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

                        var userJson = System.Text.Json.JsonSerializer.Serialize(userSession);
                        context.Session.SetString("CurrentUser", userJson);
                    }
                }
            }

            await _next(context);
        }
    }
}