﻿using Incoding.Data.Raven.Provider;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using Incoding.Data;
    using Incoding.UnitTests.MSpec;
    using Machine.Specifications;
    using Raven.Client.Document;
    using Raven.Client.Indexes;
    using Raven.Imports.Newtonsoft.Json;

    #endregion

    [Subject(typeof(RavenDbRepository))]
    public class When_raven_db_scenario
    {
        #region Fake classes

        class City
        {
            #region Constructors

            public City()
            {
                Id = Guid.NewGuid().ToString();
            }

            #endregion

            #region Properties

            public string Id { get; set; }

            public string Name { get; set; }

            public string CountryId { get; set; }

            [JsonIgnore]
            public Country Country { get; set; }

            #endregion
        }

        class Country
        {
            #region Constructors

            public Country()
            {
                Id = Guid.NewGuid().ToString();
            }

            #endregion

            #region Properties

            public string Id { get; set; }

            public string Title { get; set; }

            #endregion
        }

        class CityIndex : AbstractMultiMapIndexCreationTask<City>
        {
            #region Constructors

            public CityIndex()
            {
                AddMap<City>(cities => from city in cities
                                       select new { });
                AddMap<Country>(countries => from country in countries
                                             select new { });

                //TransformResults = (database, results) => results.Select(r => new City
                //                                                              {
                //                                                                      Id = r.Id, 
                //                                                                      Country = database.Load<Country>(r.CountryId), 
                //                                                                      CountryId = r.CountryId, 
                //                                                                      Name = r.Name
                //                                                              });
            }

            #endregion

            #region Properties

            public override string IndexName { get { return typeof(City).Name; } }

            #endregion
        }

        #endregion

        #region Establish value

        static DocumentStore documentStore;

        static City city;

        static Country country;

        #endregion

        Establish establish = () =>
                              {
                                  documentStore = new DocumentStore
                                                  {
                                                          Url = ConfigurationManager.ConnectionStrings["IncRealRavenDb"].ConnectionString, 
                                                          DefaultDatabase = "IncTest", 
                                                  };
                                  documentStore.Conventions.AllowQueriesOnId = true;
                                  documentStore.Conventions.MaxNumberOfRequestsPerSession = 1000;
                                  documentStore.Initialize();
                                  

                                  country = new Country
                                            {
                                                    Title = "Russian"
                                            };

                                  city = new City
                                         {
                                                 Name = "Taganrog", 
                                                 CountryId = country.Id, 
                                         };
                              };

        Because of = () =>
                     {
                         using (var session = documentStore.OpenSession())
                         {
                             session.Store(country);
                             session.Store(city);
                             session.SaveChanges();
                         }
                     };

        It should_be_query_city_with_index = () =>
                                             {
                                                 Thread.Sleep(1000);
                                                 using (var session = documentStore.OpenSession())
                                                 {
                                                     var cityFromDb = session.Query<City>()
                                                     .Select(r => new
                                                         {
                                                            Id = r.Id,
                                                             Country = r.Country,
                                                             CountryId = r.CountryId,
                                                             Name = r.Name
                                                     })
                                                                             .FirstOrDefault();
                                                     cityFromDb.Country.ShouldNotBeNull();
                                                 }
                                             };

        It should_be_query_city = () =>
                                  {
                                      using (var session = documentStore.OpenSession())
                                      {
                                          var cityFromDb = session.Query<City>()
                                                                  .FirstOrDefault(r => r.Id == city.Id);
                                          cityFromDb.ShouldEqualWeak(city);
                                      }
                                  };

        It should_be_query_country = () =>
                                     {
                                         using (var session = documentStore.OpenSession())
                                         {
                                             session.Query<Country>()
                                                    .FirstOrDefault(r => r.Id == country.Id)
                                                    .ShouldEqualWeak(country);
                                         }
                                     };
    }
}