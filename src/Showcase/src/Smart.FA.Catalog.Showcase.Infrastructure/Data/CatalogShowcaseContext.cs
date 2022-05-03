#nullable enable
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Showcase.Domain.Models;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Data;

public partial class CatalogShowcaseContext : DbContext
{
    public CatalogShowcaseContext()
    {
    }

    public CatalogShowcaseContext(DbContextOptions<CatalogShowcaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TrainerDetails> TrainerDetails { get; set; } = null!;
    public virtual DbSet<TrainingDetails> TrainingDetails { get; set; } = null!;
    public virtual DbSet<TrainingList> TrainingList { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrainerDetails>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("v_TrainerDetails");

            entity.Property(e => e.Biography).HasMaxLength(1500);

            entity.Property(e => e.FirstName).HasMaxLength(200);

            entity.Property(e => e.LastName).HasMaxLength(200);

            entity.Property(e => e.ProfileImagePath).HasMaxLength(50);

            entity.Property(e => e.Title).HasMaxLength(150);

            entity.Property(e => e.UrlToProfile).HasMaxLength(255);
        });

        modelBuilder.Entity<TrainingDetails>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("v_TrainingDetails");

            entity.Property(e => e.Goal).HasMaxLength(1000);

            entity.Property(e => e.Language)
                .HasMaxLength(2)
                .IsFixedLength();

            entity.Property(e => e.Methodology).HasMaxLength(1000);

            entity.Property(e => e.PracticalModalities).HasMaxLength(1000);

            entity.Property(e => e.TrainerFirstName)
                .HasMaxLength(200)
                .HasColumnName("FirstName");

            entity.Property(e => e.TrainerLastName)
                .HasMaxLength(200)
                .HasColumnName("LastName");

            entity.Property(e => e.TrainerTitle)
                .HasMaxLength(150)
                .HasColumnName("trainerTitle");

            entity.Property(e => e.TrainingTitle)
                .HasMaxLength(500)
                .HasColumnName("Title");
        });

        modelBuilder.Entity<TrainingList>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("v_TrainingList");

            entity.Property(e => e.Language)
                .HasMaxLength(2)
                .IsFixedLength();

            entity.Property(e => e.Status).HasColumnName("StatusId");

            entity.Property(e => e.Title).HasMaxLength(500);

            entity.Property(e => e.Topic).HasColumnName("TrainingTopicId");

            entity.Property(e => e.TrainerFirstName)
                .HasMaxLength(200)
                .HasColumnName("FirstName");

            entity.Property(e => e.TrainerLastName)
                .HasMaxLength(200)
                .HasColumnName("LastName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
