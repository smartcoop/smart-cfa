using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainerSocialNetworkConfiguration : IEntityTypeConfiguration<TrainerSocialNetwork>
{
    public void Configure(EntityTypeBuilder<TrainerSocialNetwork> builder)
    {
        builder.ToTable("TrainerSocialNetwork");

        builder.HasKey(trainerSocialNetwork => new
        {
            trainerSocialNetwork.TrainerId, trainerSocialNetwork.SocialNetwork
        });

        builder.HasIndex(trainerSocialNetwork => trainerSocialNetwork.SocialNetwork);
        builder.Property(trainerSocialNetwork => trainerSocialNetwork.UrlToProfile)
            .HasMaxLength(255);
    }
}
