namespace Incoding.Block.Caching
{
    #region << Using >>

    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using Incoding.Block.Core;

    #endregion

    public sealed class CachingFactory : FactoryBase<CachingInit>
    {
        #region Static Fields

        static readonly ConcurrentDictionary<string, ICacheKey> cacheBuffer = new ConcurrentDictionary<string, ICacheKey>();

        private static readonly ConcurrentDictionary<string, object> MiniLocks = new ConcurrentDictionary<string, object>();

        static readonly Lazy<CachingFactory> instance = new Lazy<CachingFactory>(() => new CachingFactory());

        #endregion

        #region Properties

        public static CachingFactory Instance { get { return instance.Value; } }

        #endregion

        #region Api Methods

        public void Delete(ICacheKey key)
        {
            this.init.Provider.Delete(key.GetName());
        }

        public void Set<TKey, TInstance>(TKey key, TInstance value, CacheOptions cacheOptions = null)
                where TInstance : new()
                where TKey : class, ICacheKey
        {
            Set(key.GetName(), value, cacheOptions);
        }

        public void Set<TInstance>(string name, TInstance value, CacheOptions cacheOptions = null)
        {
            lock (MiniLocks.GetOrAdd(name, k => new object()))
                this.init.Provider.Set(name, value, cacheOptions ?? CacheOptions.DefaultCacheOptions);
        }

        public TResult Get<TResult>(ICacheKey key)
        {
            return Get<TResult>(key.GetName());
        }

        public TResult Get<TResult>(string name)
        {
            return this.init.Provider.Get<TResult>(name);
        }

        public TResult Retrieve<TResult>(ICacheKey key, Func<TResult> callback, CacheOptions cacheOptions = null)
        {
            return Retrieve(key.GetName(), callback, cacheOptions);
        }

        public TResult Retrieve<TResult>(string name, Func<TResult> callback, CacheOptions cacheOptions = null)
        {
            TResult model = Get<TResult>(name);
            if (model == null)
            {
                object miniLock = MiniLocks.GetOrAdd(name, k => new object());
                lock (miniLock)
                {
                    model = Get<TResult>(name);
                    if (model == null)
                    {
                        model = callback();
                        if (model != null)
                            Set(name, model, cacheOptions);
                    }
                    object temp;
                    if (MiniLocks.TryGetValue(name, out temp) && (temp == miniLock))
                        MiniLocks.TryRemove(name, out temp);
                }
            }
            return model;
        }

        #endregion

        //bool IsExpires(ICacheKey key)
        //{
        //    var cachingPolicy = this.init.  GetLocalPolicy(key) ?? this.init.GetGlobalPolicy(key);
        //    return cachingPolicy != null && cachingPolicy.IsExpires(key);
        //}
    }
}