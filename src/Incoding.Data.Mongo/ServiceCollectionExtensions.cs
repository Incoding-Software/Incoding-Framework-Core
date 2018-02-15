using System;
using Incoding.Core;
using Incoding.Core.Data;
using Incoding.Data.Mongo.Provider;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Incoding.Data.Mongo
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureIncodingMongoDataServices(this IServiceCollection services, Type entityType, string url)
        {
            services.ConfigureIncodingCoreServices();

            services.AddSingleton<IUnitOfWorkFactory, MongoDbUnitOfWorkFactory>();
            
            MongoDbSessionFactory sessionFactory = new MongoDbSessionFactory(url);

            services.AddSingleton<IMongoDbSessionFactory>(sessionFactory);
            

            //var container = new Container();
            //container.Register<IDispatcher, DefaultDispatcher>();

            //container.Register<IUnitOfWorkFactory, NhibernateUnitOfWorkFactory>(Lifestyle.Singleton);

            //Configuration cfg = NhibernatePersist.CreateDefault(entityType, connectionString, version);

            //container.RegisterSingleton<INhibernateSessionFactory>(() =>
            //{
            //    FluentConfiguration configure = Fluently.Configure(cfg);
            //    return new NhibernateSessionFactory(configure);
            //});
            //container.Register<IUnitOfWorkFactory, NhibernateUnitOfWorkFactory>(Lifestyle.Singleton);

        }
    }
}