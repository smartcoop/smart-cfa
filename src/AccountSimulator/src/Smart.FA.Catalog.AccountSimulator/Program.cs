using Smart.Design.Razor.Extensions;
using Smart.FA.Catalog.AccountSimulator;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(logBuilder =>
{
    logBuilder.ClearProviders(); // removes all providers from LoggerFactory
    logBuilder.AddConsole();
    logBuilder.AddTraceSource("Information, ActivityTracing"); // Add Trace listener provider
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddSmartDesign();
builder.Services.AddDataProtection();
builder.Services.AddScoped<IAccountDataHeaderSerializer, AccountDataHeaderSerializer>();

var app = builder.Build();

app.UseProxyHeaders();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
