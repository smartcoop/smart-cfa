using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Smart.FA.Catalog.Shared.Security;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Web.Extensions;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;
using Smart.FA.Catalog.Showcase.Web.Services.Training;

var builder = WebApplication.CreateBuilder(args);

// Use NLog as logging provider
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddRazorPages();

// Add localization.
builder.Services
    .AddShowcaseLocalization()
    .AddTransient<ITrainingService, TrainingService>()
    .AddTransient<ITrainerService, TrainerService>();

builder.Services.AddDbContext<CatalogShowcaseContext>((_, efOptions) =>
{
    efOptions.UseSqlServer(builder.Configuration.GetConnectionString("Catalog"));
});

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// A fix which allows SQL domain authentication with Kerberos
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

app.Run();
