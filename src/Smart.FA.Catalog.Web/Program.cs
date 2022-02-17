using Application.Extensions;
using Infrastructure.Extensions;
using Smart.Design.Razor.Extensions;
using Web.Extensions;
using Web.Extensions.Middlewares;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");

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
    .AddFluentValidation(cfg =>
    {
        cfg.RegisterValidatorsFromAssemblyContaining<Program>();
        cfg.DisableDataAnnotationsValidation = true;
    })
    .AddViewLocalization(options => options.ResourcesPath = "Resources");

#if DEBUG
builder.Services
    .AddInfrastructure(builder.Configuration.GetConnectionString("Training"),
        builder.Configuration.GetConnectionString("Account"),
        true,
        builder.Configuration.GetSection("MailOptions"));
#else
builder.Services
    .AddInfrastructure(builder.Configuration.GetConnectionString("Training"),
    builder.Configuration.GetConnectionString("Account"),
                    false,
        builder.Configuration.GetSection("MailOptions"));
#endif

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
    builder.ApplyMigrations();
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

app.Run();
