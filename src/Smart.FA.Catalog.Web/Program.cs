using Application.Extensions;
using Application.SeedWork;
using Core.Services;
using Infrastructure.Extensions;
using Smart.Design.Razor.Extensions;
using Web.Extensions;
using Web.Extensions.Middlewares;
using FluentValidation.AspNetCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// appsettings.Local.json will have precedence over anything else as it is set in last.
// https://github.com/dotnet/aspnetcore/blob/c5207d21ed68041879e1256406b458d130b420ab/src/DefaultBuilder/src/WebHost.cs#L170
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

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
        configuration.RegisterValidatorsFromAssemblyContaining<ResponseBase>();
        configuration.DisableDataAnnotationsValidation = true;
    })
    .AddViewLocalization(options =>
    {
        options.ResourcesPath = "Resources";
    });

builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();

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
var seedingTask = app.Services.GetRequiredService<IBootStrapService>().ApplyMigrationsAndSeedAsync();

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
    app.UseStatusCodePagesWithReExecute("/{0}");
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

await seedingTask;

app.Run();
