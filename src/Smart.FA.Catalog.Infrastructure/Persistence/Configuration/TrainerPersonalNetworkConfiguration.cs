using Core.Domain;
using Core.SeedWork;
using Infrastructure.Persistence.Configuration.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

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
