namespace MS.Mvc.Models;

public class Order
{
    public string Id { get; set; }
    public string Status { get; set; }
    public string UserId { get; set; }
    public MSUser User { get; set; }
    public DateTime CreationDate { get; set; }
    public string Address { get; set; }
    public string Comments { get; set; }
    public ICollection<OrderElement> Elements { get; set; } = new List<OrderElement>();


    public const string ProcessingStatus = "Processing";
    public const string GoingToStatus = "Going to";
    public const string CompletedStatus = "Completed";

}
