using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain.Authorization;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class BlackListedUserConfiguration : IEntityTypeConfiguration<BlackListedUser>
{
    public void Configure(EntityTypeBuilder<BlackListedUser> builder)
    {
        builder.Property(blackListedUser => blackListedUser.UserId).HasColumnName("UserId").HasMaxLength(200).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        builder.Property(blackListedUser => blackListedUser.ApplicationTypeId).HasColumnName("ApplicationType").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasKey(blackListedUser => new { blackListedUser.UserId, blackListedUser.ApplicationTypeId }).IsClustered();

        builder.ToTable("BlackListedUser", "Cfa");
    }
}
