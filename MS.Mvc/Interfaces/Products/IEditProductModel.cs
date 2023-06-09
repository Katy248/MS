namespace MS.Mvc.Interfaces.Products;

public interface IEditProductModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string MeasurementUnit { get; set; }
    public string MeasurementUnitAbbreviation { get; set; }
}
