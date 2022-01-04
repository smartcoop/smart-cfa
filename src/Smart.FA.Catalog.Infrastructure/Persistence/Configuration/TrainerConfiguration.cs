using Core.Domain;
using Infrastructure.Persistence.Configuration.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TrainerConfiguration : EntityConfigurationBase<Trainer>
{
    public override void Configure(EntityTypeBuilder<Trainer> builder)
    {
        base.Configure(builder);

        builder.Property(trainer => trainer.Id).ValueGeneratedOnAdd();
        builder.HasMany(trainer => trainer.Enrollments).WithOne(enrollment => enrollment.Trainer).HasForeignKey(enrollment => enrollment.TrainerId);
        builder.HasKey(trainer => trainer.Id);
        builder.Property(trainer => trainer.FirstName).HasMaxLength(128);
        builder.Property(trainer => trainer.LastName).HasMaxLength(128);
        builder.Property(trainer => trainer.Description).HasMaxLength(1500);
        builder.Property(trainer => trainer.DefaultLanguage).HasMaxLength(2).IsRequired(false);

        builder.ToTable("Trainer");
    }
}
