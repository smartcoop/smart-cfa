using Core.Domain;
using Infrastructure.Persistence.Configuration.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

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

        builder.Property(trainer => trainer.DefaultLanguage).HasConversion(language => language.Value,
            language => Language.Create(language).Value);
        builder.Property(trainer => trainer.Biography).HasMaxLength(1500);
        builder.Property(trainer => trainer.Title).HasMaxLength(150);

        builder.ToTable("Trainer");
    }
}
