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
            Action<DbContextOptionsBuilder<IncDbContext>> builderConfigure, Action<Func<string, IncDbContext>> onCreated = null)
        {
            services.AddSingleton<IUnitOfWorkFactory, EntityFrameworkUnitOfWorkFactory>();

            Func<string, IncDbContext> incDbContext = (cs) =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<IncDbContext>();
                builderConfigure(dbContextOptionsBuilder);
                return new IncDbContext(dbContextOptionsBuilder.Options, entityType.Assembly);
            };

            onCreated?.Invoke(incDbContext);
            
            EntityFrameworkSessionFactory sessionFactory = new EntityFrameworkSessionFactory(incDbContext);

            services.AddSingleton<IEntityFrameworkSessionFactory>(sessionFactory);

        }
    }
}