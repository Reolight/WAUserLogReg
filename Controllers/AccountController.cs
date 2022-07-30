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
                Console.WriteLine("Validation passed succsesfully");
                AppUser user = new AppUser
                {
                    UserName = registerVM.Name,
                    Email = registerVM.Email,
                    RegisterTime = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    Console.WriteLine("User Created");
                    await _signInManager.SignInAsync(user, false);
                    user.LastLogin = DateTime.Now;
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
                var result = await _signInManager.PasswordSignInAsync(loginVM.Name, loginVM.Password, loginVM.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong crefentials, try again");
                }
            }
            return View(loginVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            (await _userManager.FindByNameAsync(User.Identity.Name)).LastLogin = DateTime.Now;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}