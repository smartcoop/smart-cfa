using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TrainingIdentityConfigurations: IEntityTypeConfiguration<TrainingIdentity>
{
    public void Configure(EntityTypeBuilder<TrainingIdentity> builder)
    {
        builder.HasKey(identity => new {identity.TrainingId, identity.TrainingTypeId});
        builder.HasOne(identity => identity.Training)
            .WithMany(training => training.Identities)
            .HasForeignKey(identity => identity.TrainingId);

        builder.ToTable("TrainingIdentity");
    }
}
