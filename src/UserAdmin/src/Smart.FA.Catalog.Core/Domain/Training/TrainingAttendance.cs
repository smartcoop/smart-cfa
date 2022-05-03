using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain;

public class TrainingAttendance
{
    #region Properties

    public int TrainingId { get; private set; }
    public int AttendanceTypeId { get; private set; }
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
        AttendanceTypeId = attendanceType.Id;
    }

    #endregion
}
