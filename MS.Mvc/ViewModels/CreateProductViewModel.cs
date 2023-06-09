using System.ComponentModel.DataAnnotations;
using MS.Mvc.Interfaces.Products;

namespace MS.Mvc.ViewModels;

public class CreateProductViewModel : ICreateProductModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string MeasurementUnit { get; set; }
    [Required]
    public string MeasurementUnitAbbreviation { get; set; }
}
