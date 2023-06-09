namespace MS.Mvc.Models;

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int QuantityAtStock { get; set; }
    public string MeasurementUnit { get; set; }
    public string MeasurementUnitAbbreviation { get; set; }

}
