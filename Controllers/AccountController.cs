using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WAUserLogReg.Data;
using WAUserLogReg.Models;
using WAUserLogReg.ViewModel;

namespace WAUserLogReg.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> singInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = singInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = registerVM.Name,
                    Email = registerVM.Email,
                    RegisterTime = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    await SignIn(user.UserName, registerVM.Password, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(registerVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var responce = new LoginViewModel();
            return View(responce);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                if (await SignIn(loginVM.Name, loginVM.Password, loginVM.RememberMe))
                    return RedirectToAction("Index", "Home");
                
            }
            return View(loginVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task<bool> SignIn(string name, string password, bool rememberMe)
        {
            AppUser user = await _userManager.FindByNameAsync(name);
            if (user.IsBlocked)
            {
                ModelState.AddModelError("", "You were banned and can't log in :<");
            }
            
            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
            if (result.Succeeded)
            {
                user.LastLogin = DateTime.Now;
                await _userManager.UpdateAsync(user);
                return true;
            }
            else
            {
                ModelState.AddModelError("", "Wrong credentials, try again");
            }

            return false;
        }
    }
}