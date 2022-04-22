namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

public class AttendanceType: Enumeration
{
    public static readonly AttendanceType Group = new(1, nameof(Group));
    public static readonly AttendanceType Single = new(2, nameof(Single));

    public AttendanceType(int id, string name): base(id, name)
    {

    }
}
