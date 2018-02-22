using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using GridUI.Persistance;
using GridUI.Setups;
using Incoding.Core;
using Incoding.Core.Block.Caching;
using Incoding.Core.Block.Caching.Providers;
using Incoding.Core.Block.IoC;
using Incoding.Core.Block.IoC.Provider;
using Incoding.Core.CQRS;
using Incoding.Data.EF;
using Incoding.Data.EF.Provider;
using Incoding.Web.Grid.Components;
using Incoding.Web.Grid.Options;
using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Web.Grid.Demo
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
            services.AddMvc().AddFluentValidation(configuration =>
            {
                // Setting up FluentValidation Validator Factory
                configuration.ValidatorFactory = new IncValidatorFactory();

                AssemblyScanner.FindValidatorsInAssemblyContaining<User>().ForEach(result =>
                {
                    services.Add(ServiceDescriptor.Transient(result.InterfaceType, result.ValidatorType));
                    services.Add(ServiceDescriptor.Transient(result.ValidatorType, result.ValidatorType));
                });
            });
            // Configure Core services
            services.ConfigureIncodingCoreServices();

            // Configure Entity Framework (requires Incoding.Data.EF provider). You can use any existing provider implementation available in Incoding.Data.* on Nuget
            services.ConfigureIncodingEFDataServices(typeof(User), builder =>
            {
                builder.UseSqlServer(Configuration.GetConnectionString("Main"));
            });

            // Configure Incoding Framework MVC services
            services.ConfigureIncodingWebServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            app.MapGridTemplates();

            app.UseMvc(routes =>
            {
                routes.ConfigureCQRS();
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            IoCFactory.Instance.Initialize(init => init.WithProvider(new MSDependencyInjectionIoCProvider(app.ApplicationServices)));

            // Configuring Incoding Framework caching
            CachingFactory.Instance.Initialize(init => init.WithProvider(new NetCachedProvider(() => app.ApplicationServices.GetRequiredService<IMemoryCache>())));


            var ajaxDef = JqueryAjaxOptions.Default;
            ajaxDef.Cache = false; // disabled cache as default
            GridOptions.Default.NoRecordsSelector = "no records default global";
            GridOptions.Default.AddStyling(BootstrapTable.Bordered | BootstrapTable.Hover);

            app.ApplicationServices.GetRequiredService<IEntityFrameworkSessionFactory>()
                .Open(Configuration.GetConnectionString("Main"))
                .Database.EnsureCreated();

            new DefaultDispatcher().Push(new ProductsSetupCommand());
        }
    }
}
