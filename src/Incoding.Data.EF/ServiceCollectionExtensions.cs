using System;
using Incoding.Core;
using Incoding.Core.Data;
using Incoding.Data.EF.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Data.EF
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureIncodingEFDataServices(this IServiceCollection services, Type entityType, 
            Action<DbContextOptionsBuilder<IncDbContext>> builderConfigure)
        {
            services.AddSingleton<IUnitOfWorkFactory, EntityFrameworkUnitOfWorkFactory>();

//            string path = Path.Combine(AppContext.BaseDirectory, string.Format("fluently_{0}_{1}.cfg", version,
//#if DEBUG
//                new string(connectionString.ToCharArray().Select(r => Char.IsLetterOrDigit(r) ? r : 'a').ToArray())
//#else
//                ""
//#endif
//            ));

            //var dbContext = FileCaching.Evaluate(path, () =>
            //{
                Func<string, IncDbContext> incDbContext = (cs) =>
                {
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<IncDbContext>();
                    builderConfigure(dbContextOptionsBuilder);
                    return new IncDbContext(dbContextOptionsBuilder.Options, entityType.Assembly);
                };
            //    return incDbContext;
            //});
            //incDbContext(connectionString).Database.EnsureCreated();
            EntityFrameworkSessionFactory sessionFactory = new EntityFrameworkSessionFactory(incDbContext);

            services.AddSingleton<IEntityFrameworkSessionFactory>(sessionFactory);
            
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