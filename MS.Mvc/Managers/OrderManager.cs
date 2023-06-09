using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MS.Mvc.Data;
using MS.Mvc.Interfaces.Managers;
using MS.Mvc.Interfaces.Orders;
using MS.Mvc.Models;

namespace MS.Mvc.Managers;

public class OrderManager : IOrderManager
{
    private readonly ApplicationDbContext _context;
    private readonly IProductManager _productManager;
    private readonly UserManager<MSUser> _userManager;

    public OrderManager(ApplicationDbContext context, IProductManager productManager, UserManager<MSUser> userManager)
    {
        _context = context;
        _productManager = productManager;
        _userManager = userManager;
    }
    protected string? GetUserId(ClaimsPrincipal claimsPrincipal) => _userManager.GetUserId(claimsPrincipal);
    public async Task<Order?> GetById(string orderId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var order = await _context.Orders
            .Include(order => order.Elements)
            .ThenInclude(element => element.Product)
            .FirstOrDefaultAsync(order => order.Id ==  orderId, cancellationToken);

        return order;
    }

    public async Task<Order> GetNewOrder(ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken))
    {
        var orderExists = await _context.Orders
            .AnyAsync(order => string.IsNullOrWhiteSpace(order.Status) && order.UserId == GetUserId(user), cancellationToken);
        Order order;
        if (orderExists)
        {
            order = await _context.Orders
                .Include(order => order.Elements)
                .ThenInclude(element => element.Product)
                .FirstAsync(order => string.IsNullOrWhiteSpace(order.Status) && order.UserId == GetUserId(user), cancellationToken);
        }
        else
        {
            order = await CreateOrder(user, cancellationToken);
        }

        return order;
    }
    public async Task<Order> CreateOrder(ClaimsPrincipal user, CancellationToken cancellationToken = default)
    {
        var order = new Order
        {
            Id = Guid.NewGuid().ToString(),
            UserId = GetUserId(user)!,
            Address = "",
            Comments = "",
            Status = "",
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }
    public async Task<IEnumerable<Order>> GetUserOrders(ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken))
    {
        var orders = await GetOrders()
            .Where(order => order.UserId == GetUserId(user))
            .ToListAsync(cancellationToken);
        return orders;
    }

    public IQueryable<Order> GetOrders()
    {
        var orders = _context.Orders
            .Include(order => order.Elements)
            .ThenInclude(element => element.Product);
        return orders;
    }
    public async Task<IEnumerable<Order>> GetUncompletedOrders(CancellationToken cancellationToken = default(CancellationToken))
    {
        var orders = await GetOrders()
            .Where(order => order.Status != Order.CompletedStatus)
            .ToListAsync(cancellationToken);
        return orders;
    }

    public async Task<IEnumerable<Order>> GetProcessingOrders(CancellationToken cancellationToken = default(CancellationToken))
    {
        var orders = await GetOrders()
            .Where(order => order.Status == Order.ProcessingStatus)
            .ToListAsync(cancellationToken);
        return orders;
    }
    public async Task<IEnumerable<Order>> GetGoingToOrders(CancellationToken cancellationToken = default(CancellationToken))
    {
        var orders = await GetOrders()
            .Where(order => order.Status == Order.GoingToStatus)
            .ToListAsync(cancellationToken);
        return orders;
    }

    public async Task<IEnumerable<Order>> GetCompletedOrders(CancellationToken cancellationToken = default(CancellationToken))
    {
        var orders = await GetOrders()
            .Where(order => order.Status == Order.CompletedStatus)
            .ToListAsync(cancellationToken);
        return orders;
    }

    public async Task<Order> ChangeElementsInOrder(ClaimsPrincipal user, string productId, int count, CancellationToken cancellationToken = default(CancellationToken))
    {
        var order = await GetNewOrder(user, cancellationToken);

        var existingElement = order.Elements.FirstOrDefault(element => element.OrderId == order.Id && element.ProductId == productId);
        if (existingElement is not null)
        {
            existingElement.Count = count;

            if (existingElement.Count == 0)
                _context.Remove(existingElement);
        }
        else
        {
            if (count <= 0) return order;
            var product = _productManager.GetById(productId, cancellationToken);
            order.Elements.Add(new OrderElement { Id = Guid.NewGuid().ToString(), ProductId = productId, Count = count, OrderId = order.Id });
            _context.Update(order);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public Task<Order> ChangeElementsInOrder(ClaimsPrincipal user, Product product, int count, CancellationToken cancellationToken = default(CancellationToken)) =>
        ChangeElementsInOrder(user, product.Id, count, cancellationToken);

    public async Task<Order> RemoveFromOrder(ClaimsPrincipal user, string productId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var order = await GetNewOrder(user, cancellationToken);

        var elementToRemove = order.Elements.FirstOrDefault(element => element.ProductId == productId);
        order.Elements.Remove(elementToRemove);

        _context.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public Task<Order> RemoveFromOrder(ClaimsPrincipal user, Product product, CancellationToken cancellationToken = default(CancellationToken)) =>
        RemoveFromOrder(user, product.Id, cancellationToken);

    public async Task<Order?> ProcessOrder(string orderId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var order = await GetById(orderId, cancellationToken);
        if (order is null)
            return null;
        if (await CanProcessOrder(orderId, cancellationToken))
        {
            foreach (var element in order.Elements)
            {
                await _productManager.ChangeQuantity(element.ProductId, -element.Count, cancellationToken);
            }
            order.Status = Order.GoingToStatus;

            _context.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return order;
    }
    public async Task<bool> CanProcessOrder(string orderId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var order = await GetById(orderId, cancellationToken);
        if (order is null)
            return false;
        bool canProcess = true;
        foreach (var element in order.Elements)
        {
            canProcess = canProcess && await _productManager.CanChangeQuantity(element.ProductId, -element.Count, cancellationToken);
        }

        return canProcess;
    }
    public async Task<Order?> CompleteOrder(string orderId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var order = await GetById(orderId, cancellationToken);
        if (order is null)
            return null;

        order.Status = Order.CompletedStatus;
        _context.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<Order?> PublishOrder(IPublishOrderModel model, CancellationToken cancellationToken = default(CancellationToken))
    {
        var order = await GetById(model.Id, cancellationToken);
        if (order is null)
            return null;
        order.CreationDate = DateTime.Now;
        order.Address = model.Address;
        order.Comments = model.Comment ?? "";
        order.Status = Order.ProcessingStatus;
        _context.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<OrderElement?> GetElement(string elementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var element = await _context.OrderElements
            .Include(element => element.Product)
            .FirstOrDefaultAsync(element => element.Id == elementId, cancellationToken);

        return element;
    }
}
