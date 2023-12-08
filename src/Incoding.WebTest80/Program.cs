using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using FluentValidation;
using FluentValidation.AspNetCore;
using Incoding.Core;
using Incoding.Core.Data;
using Incoding.Data.NHibernate;
using Incoding.Web;
using Incoding.Web.MvcContrib;
using Incoding.WebTest80.Models;
using Incoding.WebTest80.Operations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Incoding.Core.Block.Caching;
using Incoding.Core.Block.Caching.Providers;
using Incoding.Core.Block.IoC;
using Incoding.Core.Block.IoC.Provider;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddMvc(options =>
{
    options.Filters.Add(new IncodingErrorHandlingFilter());
}).AddMvcOptions(options =>
    {
        options.ModelBinderProviders.Clear();
        options.ModelBinderProviders.Add(new FormFileModelBinderProvider());
        options.ModelBinderProviders.Add(new FormCollectionModelBinderProvider());
        options.ModelBinderProviders.Add(new ComplexObjectModelBinderProvider());
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
            builder.Services.Add(ServiceDescriptor.Transient(result.InterfaceType, result.ValidatorType));
            builder.Services.Add(ServiceDescriptor.Transient(result.ValidatorType, result.ValidatorType));
        });
    });

builder.Services.ConfigureIncodingCoreServices();

// NH Core:
string path = Path.Combine(AppContext.BaseDirectory, "fluently_" + ".cfg");
// serialization issues, do not pass path yet
Func<FluentConfiguration, FluentConfiguration> builderConfigure = configuration =>
{
    configuration = configuration.Database(MsSqlConfiguration.MsSql2012
            .ConnectionString(builder.Configuration.GetConnectionString("Main"))
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

builder.Services.AddSingleton<IUnitOfWorkFactory>(provider => new NhibernateUnitOfWorkFactory(sessionFactory));

builder.Services.AddSingleton<INhibernateSessionFactory>(sessionFactory);

NhibernateRepository.SetInterception(() => new WhereSpecInterception());

builder.Services.ConfigureIncodingWebServices();

// Add services to the container.
builder.Services.AddControllersWithViews();




// APP Initialize

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

IoCFactory.Instance.Initialize(init => init.WithProvider(new MSDependencyInjectionIoCProvider(app.Services)));
CachingFactory.Instance.Initialize(init => init.WithProvider(new NetCachedProvider(() => app.Services.GetRequiredService<IMemoryCache>())));

app.Run();
