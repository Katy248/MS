using Microsoft.EntityFrameworkCore;
using MS.Mvc.Data;
using MS.Mvc.Interfaces.Managers;
using MS.Mvc.Interfaces.Products;
using MS.Mvc.Models;

namespace MS.Mvc.Managers;

public class ProductManager : IProductManager
{
    private readonly ApplicationDbContext _context;

    public ProductManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ChangeQuantity(string productId, int change, CancellationToken cancellationToken = default)
    {
        if (change == 0)
            return;

        var product = await _context.Products.FirstOrDefaultAsync(product => product.Id == productId, cancellationToken);

        if (product is null)
            return;
        if (change < 0 && product.QuantityAtStock > Math.Abs(change))
            return;

        product.QuantityAtStock += change;

        _context.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<bool> CanChangeQuantity(string productId, int change, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(product => product.Id == productId, cancellationToken);

        if (product is null)
            return false;
        if (change < 0 && product.QuantityAtStock > Math.Abs(change))
            return false;

        return true;
    }
    public async Task<Product?> GetProduct(string id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        return product;
    }

    public async Task<Product> CreateNew(ICreateProductModel model, CancellationToken cancellationToken = default(CancellationToken))
    {
        var product = new Product 
        { 
            Id = Guid.NewGuid().ToString(), 
            Name = model.Name, 
            MeasurementUnit = model.MeasurementUnit, 
            MeasurementUnitAbbreviation = model.MeasurementUnitAbbreviation,
            QuantityAtStock = 0 
        };

        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task Delete(string productId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var product = await GetById(productId, cancellationToken);
        if (product is not null)
        {
            _context.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Product?> GetById(string productId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(product => product.Id == productId, cancellationToken);

        return product;
    }

    public async Task<Product?> Edit(IEditProductModel model, CancellationToken cancellationToken = default(CancellationToken))
    {
        var product = await GetById(model.Id, cancellationToken);
        if (product is null)
            return null;
        product.Name = model.Name;
        product.MeasurementUnit = model.MeasurementUnit;
        product.MeasurementUnitAbbreviation = model.MeasurementUnitAbbreviation;

        _context.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken = default(CancellationToken))
    {
        var products = await _context.Products.ToArrayAsync(cancellationToken);

        return products;
    }
}
