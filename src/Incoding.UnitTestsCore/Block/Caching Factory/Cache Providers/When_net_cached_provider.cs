using Incoding.Core.Block.Caching.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Machine.Specifications;

    #endregion

    [Subject(typeof(NetCachedProvider))]
    public class When_net_cached_provider : Behaviors_cached_provider
    {
        Establish establish = () =>
                                  {
                                      cachedProvider = new NetCachedProvider(() => new MemoryCache(new MemoryCacheOptions()));
                                      cachedProvider.DeleteAll();
                                  };

        Behaves_like<Behaviors_cached_provider> should_be_verify;
    }
}