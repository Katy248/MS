using System.ComponentModel.DataAnnotations;

namespace MS.Mvc.ViewModels;

public class UserLoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public bool RememberMe { get; set; } = true;
}
