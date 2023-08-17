using Microsoft.AspNetCore.Identity;

namespace ECommerce.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; }
    public int Year { get; set; }
}
