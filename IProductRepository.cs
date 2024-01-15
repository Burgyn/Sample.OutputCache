public interface IProductRepository
{
    Task<Product> GetProductAsync(int tenantId, int id);
    Task<IEnumerable<Product>> GetProductsAsync(int tenantId);
    Task<Product> AddProductAsync(int tenantId, Product product);
    Task<Product> UpdateProductAsync(int tenantId, Product product);
    Task DeleteProductAsync(int tenantId, int id);
}
