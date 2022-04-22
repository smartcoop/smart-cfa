using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Infrastructure.Persistence.Configuration.EntityConfigurations;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainerConfiguration : EntityConfigurationBase<Trainer>
{
    public override void Configure(EntityTypeBuilder<Trainer> builder)
    {
        base.Configure(builder);

        builder.Property(trainer => trainer.Id).ValueGeneratedOnAdd();
        builder.HasMany(trainer => trainer.Assignments)
            .WithOne(assignment => assignment.Trainer)
            .HasForeignKey(assignment => assignment.TrainerId);
        builder.HasKey(trainer => trainer.Id);
        builder.OwnsOne(p => p.Name, p =>
            {
                p.Property(pp => pp.FirstName).HasColumnName("FirstName").HasMaxLength(200).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                p.Property(pp => pp.LastName).HasColumnName("LastName").HasMaxLength(200).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            });

        builder.OwnsOne(p => p.Identity, p =>
        {
            p.Property(pp => pp.ApplicationTypeId).HasColumnName("ApplicationType").HasMaxLength(200).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            p.Property(pp => pp.UserId).HasColumnName("UserId").HasMaxLength(200).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        });

        builder.Property(trainer => trainer.DefaultLanguage)
            .HasConversion
            (
                language => language.Value,
                language => Language.Create(language).Value
            )
            .HasColumnType("nchar(2)");

        builder.Property(trainer => trainer.Biography).HasMaxLength(1500);
        builder.Property(trainer => trainer.Title).HasMaxLength(150);
        builder.Property(trainer => trainer.ProfileImagePath).HasMaxLength(50);

        // Maximum possible length for an email is 254.
        builder.Property(trainer => trainer.Email).HasMaxLength(254);

        builder.HasMany(trainer => trainer.PersonalSocialNetworks)
            .WithOne(socialNetwork => socialNetwork.Trainer)
            .HasForeignKey(personalSocialNetwork => personalSocialNetwork.TrainerId);

        builder.ToTable("Trainer");
    }
}
