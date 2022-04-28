using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain.Authorization;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class SuperAdminConfiguration : IEntityTypeConfiguration<SuperAdmin>
{
    public void Configure(EntityTypeBuilder<SuperAdmin> builder)
    {
        builder.ToTable("SuperAdmin", "Cfa");

        builder.HasKey(superAdmin => superAdmin.UserId);

        builder.Property(superAdmin => superAdmin.UserId).HasMaxLength(50);
    }
}
