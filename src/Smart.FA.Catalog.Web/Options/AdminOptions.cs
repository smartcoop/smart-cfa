namespace Api.Options;

public class AdminOptions
{
    public const string SectionName = "Admin";

    public TrainingOptions? Training { get; set; }
}

public class TrainingOptions
{
    public const string SectionName = $"{AdminOptions.SectionName}:Training";

    public int NumberOfTrainingsListed { get; set; }
}


