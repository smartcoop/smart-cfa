using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainerApprovalConfiguration : IEntityTypeConfiguration<TrainerApproval>
{
    public void Configure(EntityTypeBuilder<TrainerApproval> builder)
    {
        builder.HasKey(approval => new {approval.TrainerId, approval.UserChartId});
        builder.HasOne(approval => approval.Trainer)
            .WithMany(trainer => trainer.Approvals)
            .HasForeignKey(approval => approval.TrainerId);

        builder.HasOne(approval => approval.UserChartRevision)
            .WithMany(trainer => trainer.TrainerApprovals)
            .HasForeignKey(approval => approval.UserChartId);

        builder.ToTable("TrainerApproval");
    }
}
