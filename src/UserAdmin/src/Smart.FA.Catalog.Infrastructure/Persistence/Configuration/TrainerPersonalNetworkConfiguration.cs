using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence.Configuration.EntityConfigurations;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainerPersonalNetworkConfiguration : EntityConfigurationBase<PersonalSocialNetwork>
{
    public override void Configure(EntityTypeBuilder<PersonalSocialNetwork> builder)
    {
        base.Configure(builder);

        // We don't want to have a table just called PersonalSocialNetwork.
        // Otherwise it could be not clear to whom it is linked.
        builder.ToTable("TrainerPersonalNetwork");

        builder.HasKey(personalSocialNetwork => new
        {
            personalSocialNetwork.TrainerId, personalSocialNetwork.SocialNetwork
        });

        builder.HasIndex(personalSocialNetwork => personalSocialNetwork.SocialNetwork);

        builder
            .Property(personalSocialNetwork => personalSocialNetwork.SocialNetwork)
            .HasConversion
            (
                socialNetwork => socialNetwork.Id,
                socialNetworkId => Enumeration.FromValue<SocialNetwork>(socialNetworkId)
            );

        builder.Property(p => p.UrlToProfile)
            .HasMaxLength(255);
    }
}
