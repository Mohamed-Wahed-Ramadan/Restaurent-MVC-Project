//using Microsoft.EntityFrameworkCore;
//using Restaurent.Context;

//namespace Restaurent
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // ✅ Add services to the container BEFORE building app
//            builder.Services.AddControllersWithViews();

//            // ✅ Register DbContext here (not after Build)
//            //builder.Services.AddDbContext<AppDpContext>(options =>
//            //    options.UseSqlServer(Connection.DataSource));

//            var app = builder.Build();

//            // Configure the HTTP request pipeline
//            if (!app.Environment.IsDevelopment())
//            {
//                app.UseExceptionHandler("/Home/Error");
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();

//            app.UseAuthorization();

//            app.MapControllerRoute(
//                name: "default",
//                pattern: "{controller=MenuProduct}/{action=GetAll}/{id?}");

//            app.Run();
//        }
//    }
//}
/////////////////////////////////////////////////
// ============================================
// Program.cs
// ============================================
using Microsoft.EntityFrameworkCore;
using Restaurent.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<AppDpContext>();

// Add Session Support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable Session (IMPORTANT: Must be before UseAuthorization)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MenuProduct}/{action=GetAll}/{id?}");

app.Run();