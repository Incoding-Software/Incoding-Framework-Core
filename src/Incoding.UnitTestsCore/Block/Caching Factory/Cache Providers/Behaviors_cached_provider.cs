using Incoding.Core.Block.Caching.Providers;

namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Incoding.UnitTests.MSpec;
    using Machine.Specifications;

    #endregion

    [Behaviors]
    public class Behaviors_cached_provider
    {
        #region Fake classes

        [DataContract, Serializable]
        public class FakeComplexityResponse
        {
            #region Properties

            [DataMember]
            public string Test { get; set; }

            [DataMember]
            public List<FakeComplexityResponseTeacher> Teachers { get; set; }

            #endregion

            #region Nested classes

            [Serializable, DataContract]
            public class FakeComplexityResponseTeacher
            {
                #region Properties

                [DataMember]
                public string ImageThumbnailId { get; set; }

                [DataMember]
                public string First { get; set; }

                [DataMember]
                public string Last { get; set; }

                [DataMember]
                public string FatherName { get; set; }

                [DataMember]
                public string Status { get; set; }

                [DataMember]
                public string Decription { get; set; }

                [DataMember]
                public string Phone { get; set; }

                #endregion
            }

            #endregion
        }

        #endregion

        #region Establish value

        protected static FakeSerializeObject valueToCache = Pleasure.Generator.Invent<FakeSerializeObject>();

        protected static ICachedProvider cachedProvider;

        #endregion

        It should_be_delete = () =>
                              {
                                  cachedProvider.Set(new FakeCacheKey().GetName(), valueToCache, new CacheOptions());
                                  cachedProvider.Delete(new FakeCacheKey().GetName());

                                  cachedProvider.Get<FakeSerializeObject>(new FakeCacheKey().GetName()).ShouldBeNull();
                              };

        It should_be_delete_all = () =>
                                  {
                                      cachedProvider.Set(new FakeCacheKey().GetName(), valueToCache, new CacheOptions());
                                      cachedProvider.Set(new FakeCacheCustomHierarchy().GetName(), new FakeSerializeObject(), new CacheOptions());


                                      cachedProvider.Get<FakeSerializeObject>(new FakeCacheKey().GetName()).ShouldNotBeNull();
                                      cachedProvider.DeleteAll();
                                      cachedProvider.Get<FakeSerializeObject>(new FakeCacheCustomHierarchy().GetName()).ShouldBeNull();
                                  };

        It should_be_found_in_cached = () =>
                                       {
                                           cachedProvider.Set(new FakeCacheKey().GetName(), valueToCache, new CacheOptions());
                                           Pleasure.Do10((i) => cachedProvider.Get<FakeSerializeObject>(new FakeCacheKey().GetName()).ShouldEqualWeak(valueToCache));
                                       };

        It should_be_serialization = () =>
                                     {
                                         var key = new FakeCacheKey("7D668178-0A76-4E47-9FE9-C6ECD47383DB");
                                         var instance = Pleasure.Generator.Invent<FakeComplexityResponse>(dsl => dsl.GenerateTo(r => r.Teachers));
                                         cachedProvider.Set(key.GetName(), instance, new CacheOptions());
                                         cachedProvider.Get<FakeComplexityResponse>(key.GetName()).ShouldEqualWeak(instance);
                                     };
    }
}