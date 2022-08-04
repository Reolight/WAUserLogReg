using WAUserLogReg.Models;

namespace WAUserLogReg.ViewModel;

public class AdminViewModel
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? LastLogin { get; set; }
    public string? RegistrationTime { get; set; }
    public bool? isBanned { get; set; }
    
    public AdminViewModel() { }

    public AdminViewModel(AppUser user)
    {
        Name = user.UserName;
        Email = user.Email;
        LastLogin = user.LastLogin.ToString();
        RegistrationTime = user.RegisterTime.ToString();
        isBanned = user.IsBlocked;
    }

    public static IEnumerable<AdminViewModel> GetViewModels(IEnumerable<AppUser> appUsers)
    {
        List<AdminViewModel> users = new List<AdminViewModel>();

        foreach (AppUser appUser in appUsers)
        {
            users.Add(new AdminViewModel(appUser));
        }

        return users;
    }
}