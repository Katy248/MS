using System.ComponentModel.DataAnnotations;
using MS.Mvc.Models;

namespace MS.Mvc.ViewModels;

public class ChangeQuantityProductViewModel
{
    [Required]
    public string Id { get; set; }
    [Required, Range(0, int.MaxValue)]
    public int ChangeAmount { get; set; }
    public Product? Product{ get; set; }
}
