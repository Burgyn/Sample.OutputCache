using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

internal class MultiTenantCachePolicy : IOutputCachePolicy
{
    public static MultiTenantCachePolicy Instance { get; } = new();

    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        var attemptOutputCaching = AttemptOutputCaching(context);
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = attemptOutputCaching;
        context.AllowCacheStorage = attemptOutputCaching;
        context.AllowLocking = true;
        context.CacheVaryByRules.QueryKeys = "*";

        // Simple tenant resolution for demo purposes only - do not use in production 👇
        int tenantId = ResolveTenantId(context.HttpContext);

        // Add tenantId to all tags 👇
        var tags = new HashSet<string>();
        foreach (var tag in context.Tags)
        {
            tags.Add($"tenant:{tenantId}:{tag}");
        }
        context.Tags.UnionWith(tags);

        // add tenantId as a tag 👇
        context.Tags.Add($"tenant:{tenantId}");

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        var response = context.HttpContext.Response;

        if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        if (response.StatusCode != StatusCodes.Status200OK)
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
        => ValueTask.CompletedTask;

    private static bool AttemptOutputCaching(OutputCacheContext context)
    {
        var request = context.HttpContext.Request;

        if (!HttpMethods.IsGet(request.Method) && !HttpMethods.IsHead(request.Method))
        {
            return false;
        }

        return true;
    }

    private int ResolveTenantId(HttpContext context)
    {
        var tenantId = context.Request.RouteValues["tenantId"];
        return Convert.ToInt32(tenantId);
    }
}