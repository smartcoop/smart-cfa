namespace Smart.FA.Catalog.Web.Options;

public class SuperUserOptions
{
    public const string SectionName = "SuperUser";

    /// <summary>
    /// Default value is 20.
    /// </summary>
    public int NumberOfTrainingsPerPage { get; set; } = 20;
}
