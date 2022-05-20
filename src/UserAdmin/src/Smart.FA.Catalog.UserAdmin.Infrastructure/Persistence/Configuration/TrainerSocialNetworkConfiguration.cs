using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;

namespace Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence.Configuration;

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

        builder
            .Property(trainerSocialNetwork => trainerSocialNetwork.SocialNetwork)
            .HasConversion
            (
                socialNetwork => socialNetwork.Id,
                socialNetworkId => SocialNetwork.FromValue(socialNetworkId)
            ).HasColumnName($"{nameof(SocialNetwork)}Id");

        builder.Property(trainerSocialNetwork => trainerSocialNetwork.UrlToProfile)
            .HasMaxLength(255);
    }
}
