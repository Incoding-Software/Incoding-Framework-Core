using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Incoding.Block.Caching;
using Incoding.Block.IoC;
using Incoding.Core;
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
                //configuration.RegisterValidatorsFromAssemblyContaining<ItemEntity>();
            })
            //.AddRazorOptions(options =>
            //{
            //    options.PageViewLocationFormats.Insert(0, "{0}");
            //})
            ;
            services.ConfigureIncodingCoreServices();
            services.ConfigureIncodingEFDataServices(typeof(ItemEntity), builder =>
            {
                builder.UseSqlServer(Configuration.GetConnectionString("Main"));
            });
            services.ConfigureIncodingWebServices();
            IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v3;
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            //var serviceProvider = services.BuildServiceProvider();
                        
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

            IoCFactory.Instance.Initialize(app.ApplicationServices);
            CachingFactory.Instance.Initialize(init => init.WithProvider(new NetCachedProvider(() => app.ApplicationServices.GetRequiredService<IMemoryCache>())));

            BackendTaskFactory.Instance.AddExecutor("SomeService", 
                () =>
                {
                    new DefaultDispatcher().Push(new BackgroundServiceCommand());
                }, options => options.Interval = TimeSpan.FromSeconds(15));

            BackendTaskFactory.Instance.AddSequentalExecutor("Some Sequential Service", 
                new SequentialTestQuery(), arg => new SequentialTestCommand()
                , options => options.Interval = TimeSpan.FromSeconds(15));

            BackendTaskFactory.Instance.Initialize(applicationLifetime);
        }
    }
}
