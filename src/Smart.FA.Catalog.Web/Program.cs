using Api.Extensions;
using Application.Extensions;
using Infrastructure.Extensions;
using Smart.Design.Razor.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");


builder.Services.AddApplication();
builder.Services.AddSmartDesign();
builder.Services.AddRazorPages()
                .AddViewLocalization(options => options.ResourcesPath = "Resources");


#if DEBUG
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Training"), true, builder.Configuration.GetSection("MailOptions"));
#else
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Training"), false, builder.Configuration.GetSection("MailOptions"));
#endif

var app = builder.Build();


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
