using MS.Mvc.Models;

namespace MS.Mvc.ViewModels;

public class OrderHistoryViewModel
{
    public MSUser User { get; set; }
    public IEnumerable<Order> Orders{ get; set; }
}
