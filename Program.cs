using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IProductRepository, DummyProductRepository>();

builder.Services.AddOutputCache(c =>
{
    //c.AddPolicy("products", builder =>
    //    builder.Expire(TimeSpan.FromSeconds(20))
    //    .Tag("products")
    //    .AddPolicy<MultiTenantCachePolicy>());

    c.AddMultitenantPolicy("products", builder =>
           builder.Expire(TimeSpan.FromSeconds(20))
           .Tag("products"));
});

var app = builder.Build();

app.UseOutputCache();

app.MapGet("/products/{tenantId}", async (int tenantId, IProductRepository repository)
    => await repository.GetProductsAsync(tenantId))
    .CacheOutput("products");

app.MapGet("/products/{tenantId}/{id}", async (int tenantId, int id, HttpResponse response, IProductRepository repository) =>
{
    var product = await repository.GetProductAsync(tenantId, id);
    response.Headers.ETag = $"\"{product.LastModified.Ticks}\"";

    return product;
}).CacheOutput("products");

app.MapPost("/products/{tenantId}",
    async (int tenantId, Product product, IProductRepository repository, IOutputCacheStore cache, CancellationToken token) =>
{
    var newProduct = await repository.AddProductAsync(tenantId, product);
    //await cache.EvictByTagAsync($"tenant:{tenantId}:products", token);
    await cache.EvictMultitenantByTagAsync(tenantId, "products", token);

    return newProduct;
});

app.Run();
