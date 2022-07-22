using NLog.Web;
using Smart.Extensions.DependencyInjection;
using Smart.FA.Catalog.Shared.Security;
using Smart.FA.Catalog.Showcase.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// appsettings.Local.json will have precedence over anything else as it is set in last.
// https://github.com/dotnet/aspnetcore/blob/c5207d21ed68041879e1256406b458d130b420ab/src/DefaultBuilder/src/WebHost.cs#L170
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Use NLog as logging provider
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddRazorPages();

// Add every dependencies required by the Showcase web app.
builder.Services.AddDependencies(builder.Configuration, builder.Environment);

builder.Services.ConfigureShowcaseOptions(builder.Configuration);

var app = builder.Build();

// A fix which allows SQL domain authentication with Kerberos on linux host
NetSecurityNativeFix.Initialize(app.Services.GetRequiredService<ILogger<Program>>());

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/500");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

if (!app.Environment.IsProduction() && !app.Environment.IsEnvironment("PreProduction"))
{
    await app.BootStrapAsync();
}

app.Run();
