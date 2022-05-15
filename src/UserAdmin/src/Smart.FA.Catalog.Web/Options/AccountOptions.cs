namespace Smart.FA.Catalog.Web.Options;

public class AccountOptions
{
    public const string SectionName = "Account";

    public string BaseUrl { get; set; }

    public string HomePageRelativePath { get; set; }

    public string LogoutPageRelativePath { get; set; }

    public string HomePageAbsoluteUrl => Path.Combine(BaseUrl, HomePageRelativePath);

    public string LogoutPageAbsoluteUrl => Path.Combine(BaseUrl, LogoutPageRelativePath);
}
