﻿using System;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Utils;
using FluentValidation;
using FluentValidation.AspNetCore;
using Incoding.Core;
using Incoding.Core.Block.Caching;
using Incoding.Core.Block.Caching.Providers;
using Incoding.Core.Block.IoC;
using Incoding.Core.Block.IoC.Provider;
using Incoding.Core.Block.Scheduler;
using Incoding.Core.Block.Scheduler.Command;
using Incoding.Core.Block.Scheduler.Query;
using Incoding.Core.CQRS;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Extensions.LinqSpecs;
using Incoding.Core.Tasks;
using Incoding.Data;
using Incoding.Data.NHibernate;
using Incoding.Data.NHibernate.Provider;
using Incoding.Web;
using Incoding.Web.MvcContrib;
using Incoding.WebTest.Models;
using Incoding.WebTest.Operations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Incoding.WebTest
{
    public class IncodingStartup
    {
        public IncodingStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddMvc(options =>
            {
                options.Filters.Add(new IncodingErrorHandlingFilter());
            })
                .AddMvcOptions(options =>
                {
                    options.ModelBinderProviders.Clear();
                    options.ModelBinderProviders.Add(new FormFileModelBinderProvider());
                    options.ModelBinderProviders.Add(new FormCollectionModelBinderProvider());
                    options.ModelBinderProviders.Add(new ComplexTypeModelBinderProvider());
                    options.ModelBinderProviders.Add(new SimpleTypeModelBinderProvider());
                    options.ModelBinderProviders.Add(new ArrayModelBinderProvider());
                    options.ModelBinderProviders.Add(new CollectionModelBinderProvider());
                    options.ModelBinderProviders.Add(new DictionaryModelBinderProvider());
                    options.ModelBinderProviders.Add(new FloatingPointTypeModelBinderProvider());
                    //options.ModelBinderProviders.Add(new BinderTypeModelBinderProvider());
                })
            .AddFluentValidation(configuration =>
            {
                configuration.ValidatorFactory = new IncValidatorFactory();

                AssemblyScanner.FindValidatorsInAssemblyContaining<ItemEntity>().ForEach(result =>
                {
                    services.Add(ServiceDescriptor.Transient(result.InterfaceType, result.ValidatorType));
                    services.Add(ServiceDescriptor.Transient(result.ValidatorType, result.ValidatorType));
                });
            });

            services.ConfigureIncodingCoreServices();

            // EF Core:
            //services.ConfigureIncodingEFDataServices(typeof(ItemEntity), builder =>
            //{
            //    builder.UseSqlServer(Configuration.GetConnectionString("Main"));
            //});

            // NH Core:
            string path = Path.Combine(AppContext.BaseDirectory, "fluently_" + ".cfg");
            // serialization issues, do not pass path yet
            Func<FluentConfiguration, FluentConfiguration> builderConfigure = configuration =>
            {
                configuration = configuration.Database(MsSqlConfiguration.MsSql2012
                        .ConnectionString(Configuration.GetConnectionString("Main"))
                        .ShowSql())
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true));
                return configuration;
            };
            services.AddSingleton<IProviderExtension, ProviderExtension>();
            //services.ConfigureIncodingNhDataServices(typeof(ItemEntity), path, builderConfigure);

            Func<Configuration> config = () =>
            {
                FluentConfiguration fluently = Fluently.Configure();
                fluently = builderConfigure(fluently);
                fluently = fluently
                    .Mappings(configuration => configuration.FluentMappings
                        .Add(typeof(DelayToSchedulerNhMap))
                        .AddFromAssembly(typeof(ItemEntity).Assembly))
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                    ;
                        
                return fluently.BuildConfiguration();
            };

            NhibernateSessionFactory sessionFactory = new NhibernateSessionFactory(config, path);

            services.AddSingleton<IUnitOfWorkFactory>(provider => new NhibernateUnitOfWorkFactory(sessionFactory));

            services.AddSingleton<INhibernateSessionFactory>(sessionFactory);

            //second config

            //Func<FluentConfiguration, FluentConfiguration> builderConfigure2 = configuration =>
            //{
            //    configuration = configuration.Database(MsSqlConfiguration.MsSql2012
            //            .ConnectionString(Configuration.GetConnectionString("Main1"))
            //            .ShowSql())
            //        .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true));
            //    return configuration;
            //};
            //services.ConfigureIncodingNhDataServices(typeof(ItemEntity), path, builderConfigure);

            //services.AddSingleton<IUnitOfWorkFactory, NhibernateUnitOfWorkFactory>();

            //Func<Configuration> config2 = () =>
            //{
            //    FluentConfiguration fluently = Fluently.Configure();
            //    fluently = builderConfigure2(fluently);
            //    fluently = fluently
            //        .Mappings(configuration => configuration.FluentMappings
            //            .Add(typeof(DelayToSchedulerNhMap))
            //            .AddFromAssembly(typeof(ItemEntity).Assembly));

            //    return fluently.BuildConfiguration();
            //};

            //NhibernateSessionFactory sessionFactory2 = new NhibernateSessionFactory(config2, null);

            //services.AddScoped<INhibernateSessionFactory, INhibernateSessionFactory>("Main1");

            NhibernateRepository.SetInterception(() => new WhereSpecInterception());

            services.ConfigureIncodingWebServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            app.Use(async (ctx, next) =>
            {
                try
                {
                    await next.Invoke();
                }
                catch (Exception ex)
                {

                }
            });

            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.ConfigureCQRS();
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            IoCFactory.Instance.Initialize(init => init.WithProvider(new MSDependencyInjectionIoCProvider(app.ApplicationServices)));
            CachingFactory.Instance.Initialize(init => init.WithProvider(new NetCachedProvider(() => app.ApplicationServices.GetRequiredService<IMemoryCache>())));

            //var task = new DefaultDispatcher().PushAsync(new ScheduleCommand()
            //{
            //    Command = new AddItemCommand
            //    {
            //        OriginalValue = "456"
            //    },
            //    Recurrency = new GetRecurrencyDateQuery
            //    {
            //        StartDate = DateTime.UtcNow.AddMinutes(5)
            //    }
            //});
            //task.Wait();

            BackgroundTaskFactory.Instance.AddScheduler();

            BackgroundTaskFactory.Instance.AddExecutor("SomeService",
                async () =>
                {
                    await new DefaultDispatcher().PushAsync(new BackgroundServiceCommand());
                }, options => options.Interval = TimeSpan.FromSeconds(9));

            BackgroundTaskFactory.Instance.AddSequentialExecutor("Some Sequential Service",
                () => new SequentialTestQuery(), arg => new SequentialTestCommand()
                , options => options.Interval = TimeSpan.FromSeconds(25));

            BackgroundTaskFactory.Instance.Initialize();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                BackgroundTaskFactory.Instance.StopAll();
            });
        }
    }

    public class WhereSpecInterception : IRepositoryInterception
    {
        public Specification<TEntity> WhereSpec<TEntity>(Specification<TEntity> spec) where TEntity : class, IEntity, new()
        {
            if (typeof(TEntity).HasInterface(typeof(IName)))
            {
                spec = ValidSpec(spec);
            }

            return spec;
        }

        public Specification<TEntity> ValidSpec<TEntity>(Specification<TEntity> spec) where TEntity : class, IEntity, new()
        {
            var nameSpecType = typeof(NameSpec<>).MakeGenericType(typeof(TEntity));
            var nameSpec = Activator.CreateInstance(nameSpecType);
            //Specification<TEntity> nameSpec = (Specification<TEntity>)(object)new NameSpec<TEntity>();
            var newSpec = nameSpec as Specification<TEntity>;
            return spec == null ? newSpec : spec.And(newSpec);
        }
    }
}