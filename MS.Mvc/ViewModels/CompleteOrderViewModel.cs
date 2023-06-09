using System.ComponentModel.DataAnnotations;
using MS.Mvc.Interfaces.Orders;
using MS.Mvc.Models;

namespace MS.Mvc.ViewModels;

public class CompleteOrderViewModel : IPublishOrderModel
{
    public IEnumerable<OrderElement>? Elements { get; set; } = Array.Empty<OrderElement>();
    [Required]
    public string Id { get; set; }
    [Required]
    public string Address { get; set; }
    public string? Comment { get; set; } = "";
}
