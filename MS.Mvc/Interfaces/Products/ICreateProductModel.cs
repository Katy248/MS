namespace MS.Mvc.Interfaces.Products;

public interface ICreateProductModel
{
    public string Name { get; set; }
    public string MeasurementUnit { get; set; }
    public string MeasurementUnitAbbreviation { get; set; }
}
