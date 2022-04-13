using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Persistence.Configuration.EntityConfigurations;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class UserChartConfiguration: EntityConfigurationBase<UserChart>
{
    public override void Configure(EntityTypeBuilder<UserChart> builder)
    {
        base.Configure(builder);
        builder.Property(userChart => userChart.Title).HasColumnType("nvarchar(255)");
        builder.Property(userChart => userChart.Version).HasColumnType("nvarchar(50)");
        builder.Property(userChart => userChart.ExpirationDate).IsRequired(false).HasColumnType("datetime2(0)");
        builder.Property(userChart => userChart.ValidityDate).IsRequired(false).HasColumnType("datetime2(0)");

        builder.HasMany(userChart => userChart.TrainerApprovals)
            .WithOne(approval => approval.UserChart)
            .HasForeignKey(approval => approval.UserChartId);

        builder.ToTable("UserChart");

    }
}
