using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

public class TargetAudienceType: Enumeration<TargetAudienceType>
{
    public static readonly TargetAudienceType Employee   = new(1, nameof(Employee));
    public static readonly TargetAudienceType Student    = new(2, nameof(Student));
    public static readonly TargetAudienceType Unemployed = new(3, nameof(Unemployed));
    public static readonly TargetAudienceType Other      = new(4, nameof(Other));

    protected TargetAudienceType(int id, string name) : base(id, name)
    {
    }
}
