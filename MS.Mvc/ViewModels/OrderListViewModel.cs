using MS.Mvc.Models;

namespace MS.Mvc.ViewModels;

public class OrderListViewModel
{
    public IEnumerable<Order>? Orders { get; set; } = Array.Empty<Order>();
    public OrderSelector Selector { get; set; }
}
public enum OrderSelector
{
    Completed, Processing, Uncompleted, GoingTo
}
