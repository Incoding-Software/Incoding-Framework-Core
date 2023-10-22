using Incoding.Core.Block.IoC;
using Incoding.Core.Block.IoC.Provider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

IoCFactory.Instance.Initialize(init => init.WithProvider(new MSDependencyInjectionIoCProvider(app.Services)));

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("incodingCqrsQuery", "Cqrs/Query", (object)new
    {
        controller = "Dispatcher",
        action = "Query"
    });
    endpoints.MapControllerRoute("incodingCqrsValidate", "Cqrs/Validate/{incType}", (object)new
    {
        controller = "Dispatcher",
        action = "Validate"
    });
    endpoints.MapControllerRoute("incodingCqrsCommand", "Cqrs/Push", (object)new
    {
        controller = "Dispatcher",
        action = "Push"
    });
    endpoints.MapControllerRoute("incodingCqrsRender", "Cqrs/Render", (object)new
    {
        controller = "Dispatcher",
        action = "Render"
    });
    endpoints.MapControllerRoute("incodingCqrsFile", "Cqrs/File/{incType}", (object)new
    {
        controller = "Dispatcher",
        action = "QueryToFile"
    });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapRazorPages();

app.Run();
