using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence.Configuration.EntityConfigurations;
using Smart.FA.Catalog.Shared.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;

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

        builder
            .Property(trainerSocialNetwork => trainerSocialNetwork.SocialNetwork)
            .HasConversion
            (
                socialNetwork => socialNetwork.Id,
                socialNetworkId => Enumeration.FromValue<SocialNetwork>(socialNetworkId)
            ).HasColumnName($"{nameof(SocialNetwork)}Id");

        builder.Property(trainerSocialNetwork => trainerSocialNetwork.UrlToProfile)
            .HasMaxLength(255);
    }
}
