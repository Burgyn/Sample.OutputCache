using Microsoft.AspNetCore.OutputCaching;

public static class OutputCacheExtensions
{
    public static void AddMultitenantPolicy(this OutputCacheOptions options, string name, Action<OutputCachePolicyBuilder> build)
    {
        options.AddPolicy(name, b =>
        {
            build(b);
            b.AddPolicy<MultiTenantCachePolicy>();
        });
    }
}