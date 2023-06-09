using System.ComponentModel.DataAnnotations;
using MS.Mvc.Models;

namespace MS.Mvc.ViewModels;

public class OrderElementViewModel
{
    public IEnumerable<Product> Products { get; set; } = Array.Empty<Product>();
    [Required]
    public string ProductId { get; set; }
    [Range(0, int.MaxValue)]
    public int Count { get; set; }
}
