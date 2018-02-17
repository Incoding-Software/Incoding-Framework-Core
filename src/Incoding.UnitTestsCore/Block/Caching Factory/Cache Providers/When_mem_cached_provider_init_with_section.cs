using Enyim.Caching.Configuration;
using Incoding.Core.Caching.Memcached;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Machine.Specifications;

    #endregion

    [Subject(typeof(MemCachedProvider))]
    public class When_mem_cached_provider_init_with_section : Behaviors_cached_provider
    {
        Establish establish = () =>
                                  {
                                      cachedProvider = new MemCachedProvider(new MemcachedClientConfiguration(new NullLoggerFactory(), new MemcachedClientOptions()));
                                      cachedProvider.DeleteAll();
                                  };

        Behaves_like<Behaviors_cached_provider> should_be_verify;
    }
}