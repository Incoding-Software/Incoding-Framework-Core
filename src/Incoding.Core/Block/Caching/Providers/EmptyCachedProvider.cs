namespace Incoding.Block.Caching
{
    #region << Using >>

    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    #endregion

    ////ncrunch: no coverage start
    [UsedImplicitly, ExcludeFromCodeCoverage]
    internal class EmptyCachedProvider : ICachedProvider
    {
        #region ICachedProvider Members

        public void Delete(string key) { }

        public void DeleteAll() { }

        public void Set<T>(string key, T instance, CacheOptions cacheOptions) { }
        
        public T Get<T>(string name)
        {
            return default(T);
        }

        #endregion
    }

    ////ncrunch: no coverage end
}