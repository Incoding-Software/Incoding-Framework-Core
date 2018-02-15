using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Incoding.Block.Caching;
using Incoding.Core;
using Incoding.Core.Block.Caching;
using Incoding.Core.Block.IoC;
using Incoding.Core.Block.IoC.Provider;
using Incoding.Core.CQRS;
using Incoding.CQRS;
using Incoding.Data;
using Incoding.Data.EF;
using Incoding.Mvc.MvcContrib.Core;
using Incoding.Web;
using Incoding.Web.MvcContrib.IncHtmlHelper;
using Incoding.Web.Tasks;
using Incoding.WebTest.Operations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Incoding.WebTest
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
            services.AddMvc(options =>
            {
                options.Filters.Add(new ErrorHandlingFilter());
            }).AddFluentValidation(configuration =>
            {
                configuration.ValidatorFactory = new IncValidatorFactory();

                AssemblyScanner.FindValidatorsInAssemblyContaining<ItemEntity>().ForEach(result =>
                {
                    services.Add(ServiceDescriptor.Transient(result.InterfaceType, result.ValidatorType));
                    services.Add(ServiceDescriptor.Transient(result.ValidatorType, result.ValidatorType));
                });
            })
            ;
            services.ConfigureIncodingCoreServices();
            services.ConfigureIncodingEFDataServices(typeof(ItemEntity), builder =>
            {
                builder.UseSqlServer(Configuration.GetConnectionString("Main"));
            });
            services.ConfigureIncodingWebServices();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
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
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            IoCFactory.Instance.Initialize(init => init.WithProvider(new MSDependencyInjectionIoCProvider(app.ApplicationServices)));
            CachingFactory.Instance.Initialize(init => init.WithProvider(new NetCachedProvider(() => app.ApplicationServices.GetRequiredService<IMemoryCache>())));

            BackgroundTaskFactory.Instance.AddExecutor("SomeService", 
                () =>
                {
                    new DefaultDispatcher().Push(new BackgroundServiceCommand());
                }, options => options.Interval = TimeSpan.FromSeconds(15));

            BackgroundTaskFactory.Instance.AddSequentalExecutor("Some Sequential Service", 
                new SequentialTestQuery(), arg => new SequentialTestCommand()
                , options => options.Interval = TimeSpan.FromSeconds(15));

            BackgroundTaskFactory.Instance.Initialize(applicationLifetime);
        }
    }
}
