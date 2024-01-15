using Microsoft.AspNetCore.OutputCaching;

public static class OutputStoreExtensions
{
    public static async Task EvictMultitenantByTagAsync(this IOutputCacheStore cache, int tenantId, string tag, CancellationToken token)
        => await cache.EvictByTagAsync($"tenant:{tenantId}:{tag}", token);
}