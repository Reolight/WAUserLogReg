using Microsoft.AspNetCore.Identity;
using System.Runtime;

namespace WAUserLogReg.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime? LastLogin { get; set; }

        public DateTime RegisterTime { get; set; }
        public bool isBlocked;
    }
}
