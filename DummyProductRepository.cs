using AutoBogus;
using AutoBogus.Conventions;

public class DummyProductRepository : IProductRepository
{
    private readonly Dictionary<int, Product> _products = [];

    public DummyProductRepository()
    {
        AutoFaker.Configure(c =>
        {
            c.WithConventions();
        });

        var products = AutoFaker.Create().Generate<Product>(10);

        var id = 1;
        foreach (var product in products)
        {
            product.Id = id++;
            product.TenantId = 1;
            _products.Add(product.Id, product);
        }
    }

    public Task<Product> AddProductAsync(int tenaintId, Product product)
    {
        _products.Add(product.Id, product);
        return Task.FromResult(product);
    }

    public Task DeleteProductAsync(int tenaintId, int id)
    {
        _products.Remove(id);
        return Task.CompletedTask;
    }

    public Task<Product> GetProductAsync(int tenantId, int id)
        => Task.FromResult(_products[id]);

    public Task<IEnumerable<Product>> GetProductsAsync(int tenantId)
    {
        return Task.FromResult(_products.Values.Where(p => p.TenantId == tenantId).AsEnumerable());
    }

    public Task<Product> UpdateProductAsync(int tenaintId, Product product)
    {
        _products[product.Id] = product;
        return Task.FromResult(product);
    }
}
