<a href="http://incframework.com"><img class="aligncenter size-full wp-image-1738" src="http://blog.incframework.com/upload/IncFramework-logo.png" alt="IncFramework-logo" widht="100%" height="auto"  /></a>

<p style="text-align: justify;"><strong>Incoding Framework</strong> is awesome tool for Full-stack development. Try it with our <a href="https://github.com/IncodingSoftware/get-started">Get Started guide</a></p>






# Incoding Framework

is the opensource library for rapid development web/desktop-applications. Incoding Framework can be used for resolving different kinds of issues - it supports all development life cycle. Incoding Framework helps make more things with less code:

Incoding Framework supports different design patterns
Framework provides base classes for using CQRS
Meta Language allows to create "rich" client web-applications
In framework you can find methods an classes for unit tests

## Getting Started

Configuring:
```
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
            services.AddMvc(options =>
            {
				// Configure Error Handling for mvc controllers
                options.Filters.Add(new IncodingErrorHandlingFilter());
            })
			.AddFluentValidation(configuration =>
            {
				// Setting up FluentValidation Validator Factory
                configuration.ValidatorFactory = new IncValidatorFactory();

                AssemblyScanner.FindValidatorsInAssemblyContaining<ItemEntityMap>().ForEach(result =>
                {
                    services.Add(ServiceDescriptor.Transient(result.InterfaceType, result.ValidatorType));
                    services.Add(ServiceDescriptor.Transient(result.ValidatorType, result.ValidatorType));
                });
            });
			
			// Configure Core services
            services.ConfigureIncodingCoreServices();
			
			// Configure Entity Framework (requires Incoding.Data.EF provider). You can use any existing provider implementation available in Incoding.Data.* on Nuget
            services.ConfigureIncodingEFDataServices(typeof(ItemEntity), builder =>
            {
                builder.UseSqlServer(Configuration.GetConnectionString("Main"));
            });
			
			// Configure Incoding Framework MVC services
            services.ConfigureIncodingWebServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
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

            app.UseMvc(routes =>
            {
				// Configure CQRS routes (optional)
                routes.ConfigureCQRS();
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

			// Configuring Incoding Framework IoC
            IoCFactory.Instance.Initialize(init => init.WithProvider(new MSDependencyInjectionIoCProvider(app.ApplicationServices)));
			
			// Configuring Incoding Framework caching
            CachingFactory.Instance.Initialize(init => init.WithProvider(new NetCachedProvider(() => app.ApplicationServices.GetRequiredService<IMemoryCache>())));

            BackgroundTaskFactory.Instance.AddScheduler();
			
			// Configure some background services
			/*
            BackgroundTaskFactory.Instance.AddExecutor("SomeService",
                () =>
                {
                    new DefaultDispatcher().Push(new BackgroundServiceCommand());
                }, options => options.Interval = TimeSpan.FromSeconds(15));

            BackgroundTaskFactory.Instance.AddSequentalExecutor("Some Sequential Service",
                () => new SequentialTestQuery(), arg => new SequentialTestCommand()
                , options => options.Interval = TimeSpan.FromSeconds(15));
			*/
			
			// Execute this after configuring all Tasks above (include Scheduler task)
            BackgroundTaskFactory.Instance.Initialize();

			// Don't forget to stop all tasks when application is shutting down
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                BackgroundTaskFactory.Instance.StopAll();
            });
        }
    }
```

### Prerequisites

* Incoding.Core
* Incoding.Data -> Incoding.Data.* (providers)
* Incoding.Web
* Your web application

OR

* Incoding.Core
* Incoding.Data -> Incoding.Data.* (providers)
* Your desktop/other application


### Testing

Incoding.MSpec

### Installing

Web App:
```
Nuget (including all dependencies): Incoding.Web
Nuget (EF provider): Incoding.Data.EF
```

Domain Library:
```
Nuget (including all dependencies): Incoding.Data
Nuget (EF provider): Incoding.Data.EF
```

### Usage Example
```
Coming soon
```

## Versioning

For the versions available, see the [tags on this repository](https://github.com/Incoding-Software/Incoding-Framework-Core/tags). 

## Authors

* **Vlad Kopachinsky** - *Original version* - [Incoding Framework](https://github.com/Incoding-Software/Incoding-Framework)

* **Victor Gelmutdinov** - *.NET Core migration work*

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

