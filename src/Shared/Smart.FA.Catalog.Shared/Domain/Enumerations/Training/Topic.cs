namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

public class Topic : Enumeration
{
    public static readonly Topic LanguageCourse        = new(1, nameof(LanguageCourse));
    public static readonly Topic InformationTechnology = new(2, nameof(InformationTechnology));
    public static readonly Topic SocialScience         = new(3, nameof(SocialScience));
    public static readonly Topic School                = new(4, nameof(School));
    public static readonly Topic HealthCare            = new(5, nameof(HealthCare));
    public static readonly Topic EconomyMarketing      = new(8, nameof(EconomyMarketing));
    public static readonly Topic Communication         = new(6, nameof(Communication));
    public static readonly Topic Culture               = new(7, nameof(Culture));
    public static readonly Topic Sport                 = new(9, nameof(Sport));
    public static readonly Topic Other                 = new(10, nameof(Other));

    protected Topic(int id, string name) : base(id, name)
    {
    }
}
