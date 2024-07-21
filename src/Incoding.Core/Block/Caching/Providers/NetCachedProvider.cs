using System;
using Microsoft.Extensions.Caching.Memory;

namespace Incoding.Core.Block.Caching.Providers
{
    public sealed class NetCachedProvider : ICachedProvider
    {
        private MemoryCache _memoryCache;

        private Func<IMemoryCache> _getCache;

        #region ICachedProvider Members

        public NetCachedProvider()
        {
            if (_getCache == null)
                _getCache = () => new MemoryCache(new MemoryCacheOptions());
            _memoryCache = (MemoryCache)_getCache();
        }

        public NetCachedProvider(Func<IMemoryCache> getCache) : this()
        {
            _getCache = getCache;
        }

        public T Get<T>(string name)
        {
            return (T)_memoryCache.Get(name);
        }

        public void Set<T>(string key, T instance, CacheOptions cacheOptions)
        {
            _memoryCache.Set(key, instance, cacheOptions.ValidFor);
        }

        public void DeleteAll()
        {
            _memoryCache.Compact(1.0);
            //_memoryCache = (MemoryCache)_getCache();
        }

        public void Delete(string key)
        {
            _memoryCache.Remove(key);
        }

        #endregion
    }
}