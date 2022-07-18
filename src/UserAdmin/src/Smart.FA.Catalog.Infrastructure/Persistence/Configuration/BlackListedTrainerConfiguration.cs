using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain.Authorization;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class BlackListedTrainerConfiguration : IEntityTypeConfiguration<BlackListedTrainer>
{
    public void Configure(EntityTypeBuilder<BlackListedTrainer> builder)
    {
        builder.HasKey(blackListedTrainer => blackListedTrainer.TrainerId);

        builder.ToTable("BlackListedTrainer");
    }
}
