using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WAUserLogReg.Models;

namespace WAUserLogReg.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UserManager<AppUser> userManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
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