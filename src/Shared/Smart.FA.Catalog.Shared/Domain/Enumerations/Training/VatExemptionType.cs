namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

public class VatExemptionType : Enumeration
{
    public static readonly VatExemptionType LanguageCourse = new(1, nameof(LanguageCourse));
    public static readonly VatExemptionType Professional = new(2, nameof(Professional));
    public static readonly VatExemptionType ScholarTraining = new(3, nameof(ScholarTraining));

    private VatExemptionType(int id, string name) : base(id, name)
    {
    }

    protected VatExemptionType()
    {

    }
}
