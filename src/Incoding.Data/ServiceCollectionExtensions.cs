using System;
using Incoding.Block.IoC;
using Incoding.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureIncodingEFDataServices(this IServiceCollection services, Type entityType, string connectionString, int version)
        {
            services.ConfigureIncodingCoreServices();

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
                Func<string, IncDbContext> incDbContext = (cs) => new IncDbContext(new DbContextOptionsBuilder<IncDbContext>()
                        .UseSqlServer(cs ?? connectionString)
                        .Options
                    , entityType.Assembly);
            //    return incDbContext;
            //});
            incDbContext(connectionString).Database.EnsureCreated();
            EntityFrameworkSessionFactory sessionFactory = new EntityFrameworkSessionFactory(incDbContext);

            services.AddSingleton<IEntityFrameworkSessionFactory>(sessionFactory);


            IoCFactory.Instance.Initialize(services.BuildServiceProvider());


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