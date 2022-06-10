using System.Text.RegularExpressions;

namespace Smart.FA.Catalog.Shared.Helper;

public static class UriHelper
{
    public static bool IsValidUrl(string url)
    {
        url = url.Trim();
        var pattern = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$", RegexOptions.Compiled);
        return pattern.IsMatch(url);
    }
}
