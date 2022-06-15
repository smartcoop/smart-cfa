using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainingAttendanceConfiguration: IEntityTypeConfiguration<TrainingAttendance>
{
    public void Configure(EntityTypeBuilder<TrainingAttendance> builder)
    {
        builder.HasKey(attendance => new {attendance.TrainingId, attendance.AttendanceType});
        builder.HasOne(attendance => attendance.Training)
            .WithMany(training => training.Attendances)
            .HasForeignKey(attendance => attendance.TrainingId);

        builder.ToTable("TrainingAttendance");
    }
}
