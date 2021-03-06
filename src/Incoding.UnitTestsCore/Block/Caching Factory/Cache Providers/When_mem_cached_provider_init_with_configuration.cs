using Incoding.Core.Caching.Memcached;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Enyim.Caching.Configuration;
    using Enyim.Caching.Memcached;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MemCachedProvider))]
    public class When_mem_cached_provider_init_with_configuration : Behaviors_cached_provider
    {
        Establish establish = () =>
                                  {
                                      var config = new MemcachedClientConfiguration(new NullLoggerFactory(), new MemcachedClientOptions());
                                      config.AddServer("::1", 11211);
                                      config.Protocol = MemcachedProtocol.Text;

                                      config.SocketPool.ReceiveTimeout = new TimeSpan(0, 0, 10);
                                      config.SocketPool.ConnectionTimeout = new TimeSpan(0, 0, 10);
                                      config.SocketPool.DeadTimeout = new TimeSpan(0, 0, 20);
                                      cachedProvider = new MemCachedProvider(config);

                                      cachedProvider.DeleteAll();
                                  };

        Behaves_like<Behaviors_cached_provider> should_be_verify;
    }
}