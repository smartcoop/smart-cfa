using FluentAssertions;
using Smart.FA.Catalog.Shared.Helper;
using Xunit;

namespace Smart.FA.Catalog.UnitTests.Helpers;

public class UriHelperTests
{
    [Theory]
    [InlineData("www.google.be", true)]
    [InlineData("http://www.google.be", true)]
    [InlineData("https://www.google.be", true)]
    [InlineData("google.be", true)]
    [InlineData("/..google.be", false)]
    [InlineData("¤¤¤", false)]
    [InlineData("ftp://www.google.be", false)]
    public void Validate_Given_Urls(string url, bool expected)
    {
        var isUrlValid = UriHelper.IsValidUrl(url);
        isUrlValid.Should().Be(expected);
    }
}
