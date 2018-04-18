using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Incoding.Core.Data;
using Incoding.Data.NHibernate;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Incoding.Data.EF
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureIncodingNhDataServices(this IServiceCollection services,
            Type entityType, string path,
            Func<FluentConfiguration, FluentConfiguration> builderConfigure)
        {
            services.AddSingleton<IUnitOfWorkFactory, NhibernateUnitOfWorkFactory>();

//            string path = Path.Combine(AppContext.BaseDirectory, string.Format("fluently_{0}_{1}.cfg", version,
//#if DEBUG
//                new string(connectionString.ToCharArray().Select(r => Char.IsLetterOrDigit(r) ? r : 'a').ToArray())
//#else
//                ""
//#endif
//            ));

            Func<Configuration> config = () =>
            {
                FluentConfiguration fluently = Fluently.Configure();
                fluently = builderConfigure(fluently);
                fluently = fluently
                    .Mappings(configuration => configuration.FluentMappings
                        .Add(typeof(DelayToSchedulerNhMap))
                        .AddFromAssembly(entityType.Assembly));

                return fluently.BuildConfiguration();
            };

            //var dbContext = FileCaching.Evaluate(path, () =>
            //{
                //Func<string, IncDbContext> incDbContext = (cs) =>
                //{
                //    var dbContextOptionsBuilder = new DbContextOptionsBuilder<IncDbContext>();
                //    builderConfigure(dbContextOptionsBuilder);
                //    return new IncDbContext(dbContextOptionsBuilder.Options, entityType.Assembly);
                //};
            //    return incDbContext;
            //});
            //incDbContext(connectionString).Database.EnsureCreated();
            NhibernateSessionFactory sessionFactory = new NhibernateSessionFactory(config, path);

            services.AddSingleton<INhibernateSessionFactory>(sessionFactory);
            
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