using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Mvc.Interfaces.Managers;
using MS.Mvc.ViewModels;

namespace MS.Mvc.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly IProductManager _productManager;

    public ProductsController(IProductManager productManager)
    {
        _productManager = productManager;
    }
    public async Task<IActionResult> Index()
    {
        var products = await _productManager.GetAll();
        var model = new ProductListViewModel { Products = products };
        return View(model);
    }
    public IActionResult Create() => View();
    [HttpPost]
    public async Task<IActionResult> Create([Bind] CreateProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _productManager.CreateNew(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }
    public async Task<IActionResult> Edit(string id)
    {
        var product = await _productManager.GetById(id);
        if (product is null)
            return RedirectToAction(nameof(Index));

        var model = new EditProductViewModel { Id = id, Name = product.Name };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit([Bind] EditProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _productManager.Edit(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _productManager.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
