using Microsoft.AspNetCore.Identity;

namespace userapp.Models
{
    public class Users : IdentityUser
    {
        public required string fullName { get; set; }

        public string Role { get; set; }
    }
}
