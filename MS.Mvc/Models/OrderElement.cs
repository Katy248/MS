namespace MS.Mvc.Models;

public class OrderElement
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public Product Product { get; set; }
    public string OrderId { get; set; }
    public Order Order { get; set; }
    public int Count { get; set; }
}
