using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentValidation;
using FluentValidation.AspNetCore;
using Incoding.Core;
using Incoding.Core.Block.Caching;
using Incoding.Core.Block.Caching.Providers;
using Incoding.Core.Block.IoC;
using Incoding.Core.Block.IoC.Provider;
using Incoding.Core.Data;
using Incoding.Data.NHibernate;
using Incoding.Web;
using Incoding.Web.MvcContrib;
using Incoding.WebTest30.Models;
using Incoding.WebTest30.Operations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Caching.Memory;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Incoding.Data.NHibernate.Provider;

namespace Incoding.WebTest30
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
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
                    //options.ModelBinderProviders.Add(new JsonModelBinder(new OptionsManager<MvcNewtonsoftJsonOptions>(new OptionsFactory<MvcNewtonsoftJsonOptions>(
                    //    new IConfigureOptions<MvcNewtonsoftJsonOptions>[]
                    //    {
                    //        new ConfigureOptions<MvcNewtonsoftJsonOptions>(jsonOptions => {})
                    //    },
                    //    new IPostConfigureOptions<MvcNewtonsoftJsonOptions>[]  { }
                    //    ))));
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

            NhibernateRepository.SetInterception(() => new WhereSpecInterception());

            services.ConfigureIncodingWebServices();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            IoCFactory.Instance.Initialize(init => init.WithProvider(new MSDependencyInjectionIoCProvider(app.ApplicationServices)));
            CachingFactory.Instance.Initialize(init => init.WithProvider(new NetCachedProvider(() => app.ApplicationServices.GetRequiredService<IMemoryCache>())));

        }
    }
}
