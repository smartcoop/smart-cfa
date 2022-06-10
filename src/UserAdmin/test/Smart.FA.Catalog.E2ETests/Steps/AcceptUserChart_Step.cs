using FluentAssertions;
using Smart.FA.Catalog.E2ETests.Base;
using Xunit;

namespace Smart.FA.Catalog.E2ETests.Steps;

public class AcceptUserChart_Step : E2ETestBase
{
    [Fact]
    public async Task Play()
    {
        using var playwright = await GetPlaywrightAsync();
        await using var browser = await GetBrowserAsync(playwright);
        var context = await browser.NewContextAsync();
        // Open new page
        var page = await context.NewPageAsync();

        await page.GotoAsync(BaseUrl);
        if (page.Url != $"{BaseUrl}/cfa/userchart")
        {
            throw new Exception("User chart has already been accepted");
        }
        // Check input[name="HasAcceptedUserChart"]
        await page.Locator("input[name=\"HasAcceptedUserChart\"]").CheckAsync();
        // Click button:has-text("J'accepte")
        await page.Locator("button:has-text(\"J'accepte\")").ClickAsync();

        page.Url.Should().Be($"{BaseUrl}/cfa/admin");
    }
}
