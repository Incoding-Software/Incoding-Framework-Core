using System.Data;
using Incoding.Core.Data;
using Incoding.Data.Mongo.Provider;
using Incoding.Data.Raven.Provider;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Configuration;
    using Incoding.Data;
    using Incoding.UnitTests.MSpec;
    using Machine.Specifications;
    using Raven.Client.Document;

    #endregion

    [Subject(typeof(PersistenceSpecification<>))]
    public class When_persistence_specification_with_instance
    {
        It should_be_by_start_assembly = () => new PersistenceSpecification<DbEntity>()
                                                       .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                                       .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                                       .CheckProperty(r => r.Reference)
                                                       .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                                       .VerifyMappingAndSchema();

        It should_be_entity_framework = () =>
                                        {
                                            //var dbContext = MSpecAssemblyContext.EFFluent();
                                            //dbContext.Database.AutoTransactionsEnabled = false;
                                            //dbContext.Database.LazyLoadingEnabled = true;

                                            new PersistenceSpecification<DbEntity>(PleasureForData.Factory().Create(IsolationLevel.ReadUncommitted, true))
                                                    .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                                    .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                                    .CheckProperty(r => r.Reference)
                                                    .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                                    .VerifyMappingAndSchema();
                                        };

        private static IUnitOfWorkFactory BuildMongoDb(string url, bool reload = true)
        {
            var mongoUrl = new MongoUrl(url);
            var client = new MongoClient(mongoUrl);
            if (reload)
            {
                client.DropDatabase(mongoUrl.DatabaseName);
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(IncEntityBase)))
                BsonClassMap.RegisterClassMap<IncEntityBase>(map => map.UnmapProperty(r => r.Id));

            return new MongoDbUnitOfWorkFactory(new MongoDbSessionFactory(url));
        }

        It should_be_mongo_db = () => new PersistenceSpecification<DbEntity>(BuildMongoDb(ConfigurationManager.ConnectionStrings["IncRealMongoDb"].ConnectionString).Create(IsolationLevel.ReadCommitted, true))
                                              .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                              .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                              .CheckProperty(r => r.Reference)
                                              .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                              .VerifyMappingAndSchema();

        It should_be_nhibernate = () => new PersistenceSpecification<DbEntity>()
                                                .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                                .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                                .CheckProperty(r => r.Reference)
                                                .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                                .VerifyMappingAndSchema();

        It should_be_nhibernate_without_mapping = () => Catch.Exception(() => new PersistenceSpecification<DbEntityWithoutMapping>()
                                                                                      .VerifyMappingAndSchema())
                                                             .ShouldBeAssignableTo<InternalSpecificationException>();

        public static RavenDbUnitOfWorkFactory BuildRavenDb(DocumentStore document)
        {
            return new RavenDbUnitOfWorkFactory(new RavenDbSessionFactory(document));
        }

        It should_be_raven_db = () => new PersistenceSpecification<DbEntity>(BuildRavenDb(new DocumentStore
                                                                                                          {
                                                                                                                  Url = ConfigurationManager.ConnectionStrings["IncRealRavenDb"].ConnectionString,
                                                                                                                  DefaultDatabase = "IncTest",
                                                                                                          }).Create(IsolationLevel.ReadCommitted, false))
                                              .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                              .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                              .CheckProperty(r => r.Reference)
                                              .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                              .VerifyMappingAndSchema();
    }
}