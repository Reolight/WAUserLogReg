using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using WAUserLogReg.Models;
using WAUserLogReg.ViewModel;

namespace WAUserLogReg.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, ILogger<HomeController> logger, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
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
                if (User.Identity is { IsAuthenticated: true } && willIBanThem)
                {
                    await _signInManager.SignOutAsync();
                }

                user.IsBlocked = willIBanThem;
                await _userManager.UpdateAsync(user);
            }

            return true;
        }

        public async Task<bool> FarewellDearUser(IEnumerable<AppUser> users)
        {
            foreach (AppUser user in users)
            {
                if (User.Identity is { IsAuthenticated: true })
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