using System.ComponentModel.DataAnnotations;

namespace MS.Mvc.ViewModels;

public class UserRegisterViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required, Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; }
}
