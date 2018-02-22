using Incoding.Core.Block.Caching;
using Incoding.Core.Block.Caching.Providers;

namespace Incoding.UnitTest.Block
{
    #region << Using >>
    
    using Incoding.UnitTests.MSpec;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(CachingFactory))]
    public class When_caching_factory_multi_thread_access_set_cached
    {
        #region Establish value

        protected static CachingFactory cachingFactory;

        static Mock<ICachedProvider> provider;

        #endregion

        Establish establish = () =>
                                  {
                                      provider = new Mock<ICachedProvider>();
                                      cachingFactory = new CachingFactory();
                                      cachingFactory.Initialize(caching => caching.WithProvider(provider.Object));
                                  };

        Because of = () => Pleasure.MultiThread.Do10(() => cachingFactory.Set(new FakeCacheKey(), new FakeSerializeObject(), new CacheOptions()));

        It should_be_call_set_cached_only_at_once = () => provider.Verify(r => r.Set(Pleasure.MockIt.IsStrong(nameof(FakeCacheKey)), 
            Pleasure.MockIt.IsStrong(new FakeSerializeObject()), Pleasure.MockIt.IsStrong(new CacheOptions())), Times.Exactly(10));
    }
}