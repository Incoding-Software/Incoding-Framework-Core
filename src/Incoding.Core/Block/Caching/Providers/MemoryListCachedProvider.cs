using System.Collections.Concurrent;

namespace Incoding.Block.Caching
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;

    #endregion

    public sealed class MemoryListCachedProvider : ICachedProvider
    {
        #region Fields

        readonly ConcurrentDictionary<string, object> cache;

        static MemoryListCachedProvider()
        {
            
        }

        #endregion

        #region Constructors

        public MemoryListCachedProvider()
        {
            this.cache = new ConcurrentDictionary<string, object>();
        }

        #endregion

        #region ICachedProvider Members
        
        public T Get<T>(string name)
        {
            return this.cache.ContainsKey(name) ? (T)this.cache[name] : default(T);
        }

        public void Set<T>(string key, T instance, CacheOptions cacheOptions = null)
        {
            this.cache.Set(key, instance);
        }

        public void DeleteAll()
        {
            this.cache.Clear();
        }

        public void Delete(string key)
        {
            if (this.cache.ContainsKey(key))
                ((IDictionary<string,object>)this.cache).Remove(key);
        }

        #endregion

        public void Dispose()
        {
            
        }
    }
}