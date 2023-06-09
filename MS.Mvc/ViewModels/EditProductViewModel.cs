using System.ComponentModel.DataAnnotations;
using MS.Mvc.Interfaces.Products;

namespace MS.Mvc.ViewModels;

public class EditProductViewModel : IEditProductModel
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string MeasurementUnit { get; set; }
    [Required]
    public string MeasurementUnitAbbreviation { get; set; }
}
