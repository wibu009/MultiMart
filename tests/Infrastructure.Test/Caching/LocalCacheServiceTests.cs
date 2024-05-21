using BookStack.Infrastructure.Caching;

namespace Infrastructure.Test.Caching;

public class LocalCacheServiceTests : CacheServiceTests
{
    public LocalCacheServiceTests(LocalCacheService cacheService)
        : base(cacheService)
    {
    }
}