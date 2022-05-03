using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain.Authorization;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class SuperUserConfiguration : IEntityTypeConfiguration<SuperUser>
{
    public void Configure(EntityTypeBuilder<SuperUser> builder)
    {
        builder.ToTable("SuperUser", "Cfa");

        builder.HasKey(superUser => superUser.TrainerId);

        builder.Property(superUser => superUser.TrainerId).HasMaxLength(50);
    }
}
