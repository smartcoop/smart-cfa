using Smart.Design.Razor.Extensions;
using FluentValidation.AspNetCore;
using NLog.Web;
using Smart.FA.Catalog.Application.Extensions;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Infrastructure.Extensions;
using Smart.FA.Catalog.Web.Extensions;
using Smart.FA.Catalog.Web.Extensions.Middlewares;

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
    .AddApi(builder.Configuration);
builder.Services
    .AddSmartDesign();
builder.Services
    .AddRazorPages()
    .AddFluentValidation(configuration =>
    {
        configuration.RegisterValidatorsFromAssemblyContaining<Program>();
        configuration.RegisterValidatorsFromAssemblyContaining<ResponseBase>();
        configuration.DisableDataAnnotationsValidation = true;
    });

// Add localization.
builder.Services.AddCatalogLocalization();

builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();

builder.Services
    .AddInfrastructure(builder.Configuration.GetConnectionString("Catalog"),
        builder.Configuration.GetConnectionString("Account"),
        builder.Configuration.GetSection("MailOptions"),
        builder.Configuration.GetSection("EFCore"),
        builder.Configuration.GetSection("S3Storage"));

var app = builder.Build();

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


app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

await app.ExecuteBootStrapService();

app.Run();
