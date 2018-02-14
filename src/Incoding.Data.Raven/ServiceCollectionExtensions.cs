using System;
using Incoding.Block.IoC;
using Incoding.Core;
using Incoding.Data.Raven.Provider;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Listeners;

namespace Incoding.Data.Raven
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureIncodingRavenDataServices(this IServiceCollection services, DocumentStore docSession /*Action<DbContextOptionsBuilder<IncDbContext>> builderConfigure*/)
        {
            services.ConfigureIncodingCoreServices();

            services.AddSingleton<IUnitOfWorkFactory, RavenDbUnitOfWorkFactory>();
            
            RavenDbSessionFactory sessionFactory = new RavenDbSessionFactory(docSession);

            services.AddSingleton<IRavenDbSessionFactory>(sessionFactory);
            

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