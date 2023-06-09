using System.Security.Claims;
using MS.Mvc.Interfaces.Orders;
using MS.Mvc.Models;

namespace MS.Mvc.Interfaces.Managers;

public interface IOrderManager
{
    Task<Order?> GetById(string orderId, CancellationToken cancellationToken = default);
    Task<Order> CreateOrder(ClaimsPrincipal user, CancellationToken cancellationToken = default);
    Task<Order> GetNewOrder(ClaimsPrincipal user, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetUserOrders(ClaimsPrincipal user, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetProcessingOrders(CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetGoingToOrders(CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetCompletedOrders(CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetUncompletedOrders(CancellationToken cancellationToken = default);
    Task<Order> ChangeElementsInOrder(ClaimsPrincipal user, string productId, int count, CancellationToken cancellationToken = default);
    Task<OrderElement?> GetElement(string elementId, CancellationToken cancellationToken = default);
    Task<Order> ChangeElementsInOrder(ClaimsPrincipal user, Product product, int count, CancellationToken cancellationToken = default);
    Task<Order> RemoveFromOrder(ClaimsPrincipal user, string productId, CancellationToken cancellationToken = default);
    Task<Order> RemoveFromOrder(ClaimsPrincipal user, Product product, CancellationToken cancellationToken = default);
    Task<Order?> PublishOrder(IPublishOrderModel model, CancellationToken cancellationToken = default);
    Task<Order?> ProcessOrder(string orderId, CancellationToken cancellationToken = default);
    Task<bool> CanProcessOrder(string orderId, CancellationToken cancellationToken = default);
    Task<Order?> CompleteOrder(string orderId, CancellationToken cancellationToken = default);
}
