using Microsoft.AspNetCore.Identity;

namespace FlapKap.Core.Models
{
    public class User : IdentityUser
    {
        public int Deposit { get; set; }
    }
}
