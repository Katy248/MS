using MS.Mvc.Models;

namespace MS.Mvc.ViewModels;

public class ProductListViewModel
{
    public IEnumerable<Product> Products { get; set; }
}
