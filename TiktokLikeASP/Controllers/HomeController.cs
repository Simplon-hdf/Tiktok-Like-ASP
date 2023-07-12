using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TiktokLikeASP.Models;
using Microsoft.AspNetCore.Http;

namespace TiktokLikeASP.Controllers
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
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                ViewData["Username"] = HttpContext.Session.GetString("Username");
                ViewData["SessionActiveState"] = true;
                return View();
            }
            ViewData["Username"] = "";
            ViewData["SessionActiveState"] = false;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}