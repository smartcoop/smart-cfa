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
            );

        builder.Property(trainer => trainer.Biography).HasMaxLength(1500);
        builder.Property(trainer => trainer.Title).HasMaxLength(150);

        builder.HasMany(trainer => trainer.PersonalSocialNetworks)
            .WithOne(socialNetwork => socialNetwork.Trainer)
            .HasForeignKey(personalSocialNetwork => personalSocialNetwork.TrainerId);

        builder.ToTable("Trainer");
    }
}
