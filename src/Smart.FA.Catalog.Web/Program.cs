using Application.Extensions;
using Infrastructure.Extensions;
using Smart.Design.Razor.Extensions;
using Web.Extensions;
using Web.Extensions.Middlewares;
using FluentValidation.AspNetCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");

builder.Host.UseNLog();

builder.Services
    .AddHttpContextAccessor();
builder.Services
    .AddApplication();
builder.Services
    .AddApi(builder.Configuration
        .GetSection("AdminOptions"));
builder.Services
    .AddSmartDesign();
builder.Services
    .AddRazorPages()
    .AddFluentValidation(configuration =>
    {
        configuration.RegisterValidatorsFromAssemblyContaining<Program>();
        configuration.DisableDataAnnotationsValidation = true;
    })
    .AddViewLocalization(options =>
    {
        options.ResourcesPath = "Resources";
    });


#if DEBUG
builder.Services
    .AddInfrastructure(builder.Configuration.GetConnectionString("Catalog"),
        builder.Configuration.GetConnectionString("Account"),
        true,
        builder.Configuration.GetSection("MailOptions"));
#else
builder.Services
    .AddInfrastructure(builder.Configuration.GetConnectionString("Catalog"),
    builder.Configuration.GetConnectionString("Account"),
                    false,
        builder.Configuration.GetSection("MailOptions"));
#endif

var app = builder.Build();

// We don't await this operation now but it will awaited just before app.Run()
var seedingTask = app.Services.GetRequiredService<IBootStrapService>().SeedAndApplyMigrationsAsync();

app.UseForwardedHeaders();

app.UseProxyHeaders();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/404";
        await next();
    }
});
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

await seedingTask;

app.Run();
