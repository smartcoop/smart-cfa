using NLog.Web;
using Smart.Extensions.DependencyInjection;
using Smart.FA.Catalog.Shared.Security;
using Smart.FA.Catalog.Showcase.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Use NLog as logging provider
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddRazorPages();

// Add every dependencies required by the Showcase web app.
builder.Services.AddDependencies(builder.Configuration);

builder.Services.ConfigureShowcaseOptions(builder.Configuration);

var app = builder.Build();

// A fix which allows SQL domain authentication with Kerberos on linux host
NetSecurityNativeFix.Initialize(app.Services.GetRequiredService<ILogger<Program>>());

app.UseStatusCodePagesWithReExecute("/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
