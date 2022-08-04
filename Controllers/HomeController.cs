using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using WAUserLogReg.Data;
using WAUserLogReg.Models;
using WAUserLogReg.ViewModel;

namespace WAUserLogReg.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public HomeController(UserManager<AppUser> userManager, ILogger<HomeController> logger, SignInManager<AppUser> signInManager, ApplicationDbContext _context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            this._context = _context;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity == null) return View();
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            user.LastLogin = DateTime.Now;
            _userManager.UpdateAsync(user);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        // GET
        [HttpGet("account/admin")]
        public string Admin()
        {
            return JsonConvert.SerializeObject(AdminViewModel.GetViewModels(_userManager.Users));
        }

        [HttpPost("admin/del")]
        public async Task<IActionResult> Delete([FromBody] string[] avm)
        {
            await FarewellDearUser(FormUserList(avm));
            return View("Index");
        }

        [HttpPost("admin/ban")]
        public async Task<IActionResult> Ban([FromBody] string[] avm)
        {
            await BanControl(FormUserList(avm), true);
            return View("Index");
        }
        
        [HttpPost("admin/unban")]
        public async Task<IActionResult> UnBan([FromBody] string[] avm)
        {
            await BanControl(FormUserList(avm), false);
            return View("Index");
        }

        public async Task<bool> BanControl(IEnumerable<AppUser> users, bool willIBanThem)
        {
            foreach (AppUser user in users)
            {
                user.IsBlocked = willIBanThem;
                await _userManager.UpdateAsync(user);

                if (User.Identity is { IsAuthenticated: true } && user.UserName == User.Identity.Name && willIBanThem)
                {
                    await _signInManager.SignOutAsync();
                    RedirectToAction("Index", "Home");
                }
            }

            return true;
        }

        public async Task<bool> FarewellDearUser(IEnumerable<AppUser> users)
        {
            foreach (AppUser user in users)
            {
                if (User.Identity is { IsAuthenticated: true } && User.Identity.Name == user.UserName)
                {
                    await _signInManager.SignOutAsync();
                }

                await _userManager.DeleteAsync(user);
            }
            
            return true;
        }

        private IEnumerable<AppUser> FormUserList(string[] userNames)
        {
            List<AppUser> users = new List<AppUser>();
            foreach (AppUser user in _userManager.Users)
            {
                if (userNames.Contains(user.UserName))
                {
                    users.Add(user);
                }
            }

            return users;
        }
    }
}