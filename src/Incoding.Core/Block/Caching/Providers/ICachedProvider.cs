namespace Incoding.Core.Block.Caching.Providers
{
    /// <summary>
    ///     All <c>lock</c> implement in <see cref="CachingFactory" /> then not need added lock or any multi-threaded.
    /// </summary>
    public interface ICachedProvider
    {
        /// <summary>
        ///     Delete all assign value for <c>key</c>
        /// </summary>
        /// <param name="key">
        ///     Caching <c>key</c>
        /// </param>
        void Delete(string key);

        /// <summary>
        ///     <c>Delete</c> all keys
        /// </summary>
        void DeleteAll();

        /// <summary>
        ///     Add or replace if value is exists
        /// </summary>
        void Set<T>(string key, T instance, CacheOptions cacheOptions);

        /// <summary>
        ///     Get value by caching <c>key</c>.
        ///     If value not exists return default(<c>T</c>)
        /// </summary>
        T Get<T>(string key);
    }
}