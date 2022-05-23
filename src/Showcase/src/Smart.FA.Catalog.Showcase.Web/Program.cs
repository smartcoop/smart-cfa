using EntityFrameworkCore.UseRowNumberForPaging;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Smart.FA.Catalog.Shared.Security;
using Smart.FA.Catalog.Showcase.Domain.Common.Options;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact;
using Smart.FA.Catalog.Showcase.Web.Extensions;
using Smart.FA.Catalog.Showcase.Web.Options;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;
using Smart.FA.Catalog.Showcase.Web.Services.Training;

var builder = WebApplication.CreateBuilder(args);

// Use NLog as logging provider
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddRazorPages();

// Add localization.
builder.Services
    .AddFluentEmail(builder.Configuration)
    .AddShowcaseLocalization()
    .AddTransient<ITrainingService, TrainingService>()
    .AddTransient<ITrainerService, TrainerService>()
    .AddTransient<IInquiryEmailService, InquiryEmailService>()
    .AddMemoryCache();

builder.Services.Configure<MinIOOptions>(builder.Configuration.GetSection(MinIOOptions.SectionName))
    .Configure<FluentEmailOptions>(builder.Configuration.GetSection(FluentEmailOptions.SectionName))
    .Configure<IinquiryOptions>(builder.Configuration.GetSection(IinquiryOptions.SectionName));

builder.Services.AddDbContext<CatalogShowcaseContext>((_, efOptions) =>
{
    efOptions.UseSqlServer(builder.Configuration.GetConnectionString("Catalog"), options => options.UseRowNumberForPaging());
});

builder.Services.AddHttpContextAccessor();


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

app.Run();
