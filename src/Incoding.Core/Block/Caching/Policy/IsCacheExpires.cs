using Incoding.Core.Block.Caching.Core;

namespace Incoding.Core.Block.Caching.Policy
{
    public delegate bool IsCacheExpires(ICacheKey cacheKey);
}