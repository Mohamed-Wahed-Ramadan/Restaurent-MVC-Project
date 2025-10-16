using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restaurent.Models;

namespace Restaurent.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Privacy()
        {
            TempData["User"] = "Zoz";
            return View();
        }
        public IActionResult GetTemp()
        {
            var name = TempData.Peek("User");
            return View(name);
        }

        public IActionResult SetTheme(string theme)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                Path = "/"
            };
            Response.Cookies.Append("theme", theme, options);

            return Ok(new { message = "Theme cookie set successfully!" });
        }

    }
}
