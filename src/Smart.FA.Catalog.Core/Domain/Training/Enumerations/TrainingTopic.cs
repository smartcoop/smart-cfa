using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Domain.Enumerations;

public class TrainingTopic: Enumeration
{
    public static readonly TrainingTopic LanguageCourse = new(1, nameof(LanguageCourse));
    public static readonly TrainingTopic InformationTechnology = new(2, nameof(InformationTechnology));
    public static readonly TrainingTopic SocialScience = new(3, nameof(SocialScience));
    public static readonly TrainingTopic School = new(4, nameof(School));
    public static readonly TrainingTopic Health = new(5, nameof(Health));
    public static readonly TrainingTopic Communication = new(6, nameof(Communication));
    public static readonly TrainingTopic Culture = new(7, nameof(Culture));

    protected TrainingTopic(int id, string name) : base(id, name)
    {
    }
}
