using Microsoft.AspNetCore.Mvc;
using MS.Mvc.Interfaces.Managers;
using MS.Mvc.ViewModels;
using MS.Mvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace MS.Mvc.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IOrderManager _orderManager;
    private readonly IProductManager _productManager;

    public OrdersController(IOrderManager orderManager, IProductManager productManager)
    {
        _orderManager = orderManager;
        _productManager = productManager;
    }
    public async Task<IActionResult> Create()
    {
        var order = await _orderManager.GetNewOrder(User);
        return View(order);
    }
    public async Task<IActionResult> AddElement()
    {
        var products = await _productManager.GetAll();
        var productsInOrder = (await _orderManager.GetNewOrder(User)).Elements.Select(element => element.Product);
        var model = new OrderElementViewModel { Products = products.Except(productsInOrder), };
        return View(model);
    }
    public async Task<IActionResult> EditElement(string id)
    {
        var element = await _orderManager.GetElement(id);
        var products = await _productManager.GetById(element.ProductId);
        //var productsInOrder = (await _orderManager.GetNewOrder(User)).Elements.Select(element => element.Product);
        var model = new OrderElementViewModel { Products = new[] { products }, ProductId = element.ProductId, Count = element.Count };
        return View(viewName: "AddElement", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditElement([Bind] OrderElementViewModel model)
    {
        if (ModelState.IsValid)
            await _orderManager.ChangeElementsInOrder(User, model.ProductId, model.Count);
        return RedirectToAction(nameof(Create));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"><see cref="Product"/> Id.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RemoveElement(string id)
    {
        await _orderManager.ChangeElementsInOrder(User, id, 0);
        return RedirectToAction(nameof(Create));
    }
    public async Task<IActionResult> Complete(string id)
    {
        var order = await _orderManager.GetById(id);
        var model = new CompleteOrderViewModel { Id = id, Elements = order.Elements };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Complete([Bind] CompleteOrderViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _orderManager.PublishOrder(model);

        return RedirectToAction(controllerName: "Home", actionName: "Index");
    }
    public async Task<IActionResult> All(OrderSelector selector = OrderSelector.Uncompleted)
    {
        IEnumerable<Order> orders = Array.Empty<Order>();
        switch (selector)
        {
            case OrderSelector.Completed:
                orders = await _orderManager.GetCompletedOrders();
                break;
            case OrderSelector.Processing:
                orders = await _orderManager.GetProcessingOrders();
                break;
            case OrderSelector.Uncompleted:
                orders = await _orderManager.GetUncompletedOrders();
                break;
            case OrderSelector.GoingTo:
                orders = await _orderManager.GetGoingToOrders();
                break;
        }
        var model = new OrderListViewModel { Orders = orders, Selector = selector };

        return View(model);
    }
    public async Task<IActionResult> Process(string id)
    {
        var order = await _orderManager.GetById(id);
        var canProcess = await _orderManager.CanProcessOrder(id);
        var model = new ProcessOrderViewModel { Order = order, CanProcess = canProcess };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> ProcessConfirmed(string id)
    {
        await _orderManager.ProcessOrder(id);
        return RedirectToAction("All");
    }
    public async Task<IActionResult> Deliver(string id)
    {
        var order = await _orderManager.GetById(id);
        return View(order);
    }
    [HttpPost]
    public async Task<IActionResult> DeliverConfirmed(string id)
    {
        await _orderManager.CompleteOrder(id);
        return RedirectToAction("All");
    }
    
}
