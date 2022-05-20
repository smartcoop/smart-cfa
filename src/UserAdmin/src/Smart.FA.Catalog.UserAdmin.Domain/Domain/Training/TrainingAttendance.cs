using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.UserAdmin.Domain.Domain;

public class TrainingAttendance
{
    #region Properties

    public int TrainingId { get; private set; }
    public virtual AttendanceType AttendanceType { get; } = null!;
    public virtual Training Training { get; } = null!;

    #endregion

    #region Constructors

    protected TrainingAttendance()
    {

    }

    public TrainingAttendance(Training training, AttendanceType attendanceType)
    {
        Training = training;
        AttendanceType = attendanceType;
        TrainingId = training.Id;
    }

    #endregion
}
