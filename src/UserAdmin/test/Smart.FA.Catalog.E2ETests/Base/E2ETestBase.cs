using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace Smart.FA.Catalog.E2ETests.Base;

public class E2ETestBase
{
    protected string BaseUrl { get; private set; } = null!;

    protected TimeSpan DelayBeforeStart { get; private set; }
    private BrowserTypeLaunchOptions _browserTypeLaunchOptions;

    protected E2ETestBase()
    {
        Init();
    }

    protected void Init()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables();
        var config = builder.Build();
        var service = new ServiceCollection();
        service.AddOptions();
        service.Configure<Settings>(config.GetSection(Settings.UrlSectionName));
        service.Configure<Connection>(config.GetSection(Connection.UrlSectionName));
        var serviceProvider = service.BuildServiceProvider();
        var settings = serviceProvider.GetRequiredService<IOptions<Settings>>().Value;
        var connection = serviceProvider.GetRequiredService<IOptions<Connection>>().Value;

        BaseUrl = connection.BaseUrl;
        DelayBeforeStart = TimeSpan.FromSeconds(settings.DelayBeforeStartInSeconds);

        _browserTypeLaunchOptions = new BrowserTypeLaunchOptions { SlowMo = settings.SlowMo, Headless = settings.HeadlessBrowser };
    }

    protected Task<IPlaywright> GetPlaywrightAsync()
    {
        return Playwright.CreateAsync();
    }

    protected async Task<IBrowser> GetBrowserAsync(IPlaywright playwright)
    {
        return await playwright.Chromium.LaunchAsync(_browserTypeLaunchOptions);
    }

    protected Task<IPage> GetPageAsync(IBrowser browser)
    {
        return browser.NewPageAsync();
    }
}
