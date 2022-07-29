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
        private readonly SignInManager<AppUser> _singInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> singInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _context = context;
        }

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
                    await _singInManager.SignInAsync(user, false);
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

        public IActionResult Login()
        {
            var responce = new LoginViewModel();
            return View(responce);
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(UserLoginViewModel userLoginVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        View(userLoginVM);
        //    }            
        //}
    }
}
