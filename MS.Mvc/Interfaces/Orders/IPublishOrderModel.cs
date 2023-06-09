namespace MS.Mvc.Interfaces.Orders;

public interface IPublishOrderModel
{
    public string Id { get; set; }
    public string Address { get; set; }
    public string? Comment { get; set; }
}
