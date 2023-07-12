using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TiktokLikeASP.Context;
using TiktokLikeASP.Models;
using Microsoft.AspNetCore.Http;

namespace TiktokLikeASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Store Database context on application start.
        /// </summary>
        /// <param name="appDbContext">Database context of the application</param>
        public HomeController(ApplicationDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        #region GETALLPOSTS
        [HttpGet]
        // GET: Posts
        public async Task<IActionResult> Index()
        {
            ViewData["Username"] = "";
            ViewData["SessionActiveState"] = false;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                ViewData["Username"] = HttpContext.Session.GetString("Username");
                ViewData["SessionActiveState"] = true;
            }
        
            var applicationDbContext = _context.Posts.Include(p => p.Creator);
            return View(await applicationDbContext.ToListAsync());
        }
        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}