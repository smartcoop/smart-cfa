namespace Smart.FA.Catalog.Web.Options;

public class AdminOptions
{
    public const string SectionName = "AdminOptions";

    public TrainingOptions? Training { get; set; }
}

public class TrainingOptions
{
    public int NumberOfTrainingsDisplayed { get; set; }
}
