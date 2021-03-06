using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Persistence.Configuration.EntityConfigurations;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class UserChartRevisionConfiguration: EntityConfigurationBase<UserChartRevision>
{
    public override void Configure(EntityTypeBuilder<UserChartRevision> builder)
    {
        base.Configure(builder);
        builder.Property(userChart => userChart.Title).HasColumnType("nvarchar(255)");
        builder.Property(userChart => userChart.Version).HasColumnType("nvarchar(50)");
        builder.Property(userChart => userChart.ValidFrom).HasPrecision(3);
        builder.Property(userChart => userChart.ValidUntil).IsRequired(false).HasPrecision(3);

        builder.HasMany(userChart => userChart.TrainerApprovals)
            .WithOne(approval => approval.UserChartRevision)
            .HasForeignKey(approval => approval.UserChartId);

        builder.ToTable("UserChartRevision", "Cfa");
    }
}
