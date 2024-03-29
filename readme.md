﻿🚀 ASP.NET Core Output Cache for MultiTenancy Services

[In the previous post](https://blog.burgyn.online/2024/01/15/output-cache), I showed how to use the ASP.NET Core output cache.
I mentioned that it is not sutible for multi-tenancy services (especially invalidating the cache for tenant) and that I had a solution for that in mind.

So here it is 🙋‍♂️.

The key is to create your own custom cache policy.

Implement `IOutputCachePolicy`. In the `CacheRequestAsync` method, resolve tenantId and create new tags based on it.

Register your policy, define tags and use it in your endpoints.

When you want to invalidate cache for tenant, call `EvictByTagAsync` with tag based on tenantId.

You can also simply it all with creating own extension method for `IOutputCacheStore` and `OutputCacheOptions`.

Note: if you do not have `tenantId` in the path, you will need to modify `CacheVaryByRules` to use other values (e.g. header, query string, etc.)

<a href="https://www.buymeacoffee.com/minomartiniak" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-violet.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>
