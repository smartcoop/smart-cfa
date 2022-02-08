namespace Web.Options;

public class AdminOptions
{
    public TrainingOptions? Training { get; set; }
}

public class TrainingOptions
{
    public int NumberOfTrainingsDisplayed { get; set; }
}
