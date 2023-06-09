using MS.Mvc.Interfaces.Products;
using MS.Mvc.Models;

namespace MS.Mvc.Interfaces.Managers;

public interface IProductManager
{
    Task ChangeQuantity(string productId, int change, CancellationToken cancellationToken = default);
    Task<bool> CanChangeQuantity(string productId, int change, CancellationToken cancellationToken = default);
    Task<Product> CreateNew(ICreateProductModel model, CancellationToken cancellationToken = default);
    Task Delete (string productId, CancellationToken cancellationToken = default);
    Task<Product?> GetById(string productId, CancellationToken cancellationToken = default);
    Task<Product?> Edit(IEditProductModel model, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken = default);
}
