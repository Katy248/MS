using MS.Mvc.Models;

namespace MS.Mvc.ViewModels;

public class ProcessOrderViewModel
{
    public Order Order { get; set; }
    public bool CanProcess { get; set; } = false;
}
