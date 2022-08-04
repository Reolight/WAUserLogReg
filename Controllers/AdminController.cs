using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WAUserLogReg.Data;
using WAUserLogReg.Models;
using WAUserLogReg.ViewModel;

namespace WAUserLogReg.Controllers;

[ApiController]
public class AdminController : Controller
{
    public class JsonAction
    {
        public string? method { get; set; }
        public string? url { get; set; }
        public Data? data { get; set; }
            
        public class Data
        {
            public string[]? names { get; set; }
            public string? action { get; set; }
        }
    }
    private UserManager<AppUser> _userManager { get; set; }
    private SignInManager<AppUser> _signInManager { get; set; }
    private ApplicationDbContext _context { get; set; }
    private HomeController _homeController { get; }

    public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        ApplicationDbContext context, HomeController _homeController)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        this._homeController = _homeController;
    }
}