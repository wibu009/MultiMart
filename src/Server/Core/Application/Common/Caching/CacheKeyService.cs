using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Common.Caching;

public interface ICacheKeyService : IScopedService
{
    public string GetCacheKey(string name, object id, bool includeTenantId = true);
}