using Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Restaurent.Middlewares;
using Application.Interfaces;
//using Application.Services;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Application.Mappings;

namespace Restaurent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // إضافة DbContext مع Configuration
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDpContext>(options =>
                options.UseSqlServer(connectionString));

            // إضافة Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30); // جلسة طويلة للمستخدمين العاديين
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "RestaurantSession";
            });

            // ========== تسجيل جميع الـ Repositories ==========
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMenuProductRepository, MenuProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

            //// ========== تسجيل جميع الـ Services ==========
            //builder.Services.AddScoped<ICategoryService, CategoryService>();
            //builder.Services.AddScoped<IUserService, UserService>();
            //builder.Services.AddScoped<IMenuProductService, MenuProductService>();
            //builder.Services.AddScoped<IOrderService, OrderService>();
            //builder.Services.AddScoped<ICartService, CartService>();
            //builder.Services.AddScoped<IFavoriteService, FavoriteService>();
            //builder.Services.AddScoped<IDiscountService, DiscountService>();
            //builder.Services.AddScoped<IAdminService, AdminService>();

            // ========== AutoMapper ==========
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // ========== إضافة Identity ==========
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                // تكوين إعدادات Password
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // تكوين إعدادات User
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;

                // تكوين إعدادات Lockout
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<AppDpContext>()
            .AddDefaultTokenProviders();

            // ========== تكوين Cookie للـ Authentication ==========
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.LogoutPath = "/User/Logout";
                options.AccessDeniedPath = "/User/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = true;

                // إعدادات إضافية للأمان
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ?
                    Microsoft.AspNetCore.Http.CookieSecurePolicy.None :
                    Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            });

            // ========== إضافة خدمات إضافية ==========
            builder.Services.AddHttpContextAccessor(); // للوصول إلى HttpContext في الـ Services

            // إضافة سياسات CORS إذا كنت بحاجة لها
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // ========== Configure the HTTP request pipeline ==========
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                // في بيئة التطوير، نستخدم صفحة الاستثناءات التفصيلية
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // استخدام CORS إذا تمت إضافته
            app.UseCors("AllowAll");

            // الترتيب مهم جداً: Session -> Authentication -> Authorization -> Custom Middleware
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            // Middleware مخصص للتسجيل التلقائي إذا كان موجوداً
            app.UseMiddleware<AutoLoginMiddleware>();

            // ========== تعيين routes إضافية إذا لزم الأمر ==========
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=MenuProduct}/{action=GetAll}/{id?}");

            // ========== تهيئة قاعدة البيانات (اختياري) ==========
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDpContext>();
                    // تأكد من أن قاعدة البيانات قد تم إنشاؤها وتطبيق الـ Migrations
                    context.Database.Migrate();

                    // يمكنك إضافة بيانات أولية هنا إذا لزم الأمر
                    // await SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                }
            }

            app.Run();
        }
    }
}