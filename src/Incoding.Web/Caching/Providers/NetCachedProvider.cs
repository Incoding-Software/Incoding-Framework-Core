using Microsoft.Extensions.Caching.Memory;

namespace Incoding.Block.Caching
{
    #region << Using >>

    using System.Collections;
    using System.Web;

    #endregion

    public sealed class NetCachedProvider : ICachedProvider
    {
        private IMemoryCache _memoryCache;

        #region ICachedProvider Members

        public NetCachedProvider()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public T Get<T>(ICacheKey key) where T : class
        {
            return _memoryCache.Get(key.GetName()) as T;
        }

        public void Set<T>(ICacheKey key, T instance) where T : class
        {
            _memoryCache.Set(key.GetName(), instance);
        }

        public void DeleteAll()
        {
            _memoryCache.Dispose();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public void Delete(ICacheKey key)
        {
            _memoryCache.Remove(key.GetName());
        }

        #endregion
    }
}