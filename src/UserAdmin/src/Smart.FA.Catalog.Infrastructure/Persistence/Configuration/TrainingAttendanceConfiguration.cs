using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainingAttendanceConfiguration: IEntityTypeConfiguration<TrainingAttendance>
{
    public void Configure(EntityTypeBuilder<TrainingAttendance> builder)
    {
        builder.HasKey(attendance => new {attendance.TrainingId, attendance.AttendanceType});
        builder.HasOne(attendance => attendance.Training)
            .WithMany(training => training.Attendances)
            .HasForeignKey(attendance => attendance.TrainingId);

        builder.Property(e => e.AttendanceType)
            .HasColumnName($"{nameof(AttendanceType)}Id");

        builder.ToTable("TrainingAttendance");
    }
}
