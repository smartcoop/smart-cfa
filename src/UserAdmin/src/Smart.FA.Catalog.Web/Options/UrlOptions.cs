namespace Smart.FA.Catalog.Web.Options;

public class UrlOptions
{
    public const string UrlSectionName = "Url";

    public string Home { get; set; }

    public string SignOut {get; set;}

    public string Showcase { get; set; }

    public string ShowcaseTrainingDetailsPath { get; set; }

    public string GetShowcaseTrainingDetailsUrl(int trainingId) => $"{Showcase}{ShowcaseTrainingDetailsPath}{trainingId}";

}
