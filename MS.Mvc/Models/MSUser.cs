using Microsoft.AspNetCore.Identity;

namespace MS.Mvc.Models;

public class MSUser : IdentityUser
{
    public bool IsEmployee { get; set; } = false;
}
