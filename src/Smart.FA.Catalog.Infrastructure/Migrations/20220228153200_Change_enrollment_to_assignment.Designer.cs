﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(CatalogContext))]
    [Migration("20220228153200_Change_enrollment_to_assignment")]
    partial class Change_enrollment_to_assignment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Core.Domain.Enumerations.TrainingSlotNumberType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("TrainingSlotNumberType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Group"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Single"
                        });
                });

            modelBuilder.Entity("Core.Domain.Enumerations.TrainingStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("TrainingStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Draft"
                        },
                        new
                        {
                            Id = 2,
                            Name = "WaitingForValidation"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Validated"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Cancelled"
                        });
                });

            modelBuilder.Entity("Core.Domain.Enumerations.TrainingTargetAudience", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("TrainingTargetAudience");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Employee"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Student"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Unemployed"
                        });
                });

            modelBuilder.Entity("Core.Domain.Enumerations.TrainingType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("TrainingType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "LanguageCourse"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Professional"
                        },
                        new
                        {
                            Id = 3,
                            Name = "SchoolCourse"
                        },
                        new
                        {
                            Id = 4,
                            Name = "PermanentSchoolCourse"
                        });
                });

            modelBuilder.Entity("Core.Domain.Trainer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DefaultLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Trainer", (string)null);
                });

            modelBuilder.Entity("Core.Domain.TrainerAssignment", b =>
                {
                    b.Property<int>("TrainingId")
                        .HasColumnType("int");

                    b.Property<int>("TrainerId")
                        .HasColumnType("int");

                    b.HasKey("TrainingId", "TrainerId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("TrainingId", "TrainerId"));

                    b.HasIndex("TrainerId");

                    b.ToTable("TrainerAssignment", (string)null);
                });

            modelBuilder.Entity("Core.Domain.Training", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("smallint")
                        .HasColumnName("StatusId");

                    b.HasKey("Id");

                    b.ToTable("Training", (string)null);
                });

            modelBuilder.Entity("Core.Domain.TrainingDetail", b =>
                {
                    b.Property<int>("TrainingId")
                        .HasColumnType("int");

                    b.Property<string>("Language")
                        .HasColumnType("NCHAR(2)")
                        .HasColumnName("Language");

                    b.Property<string>("Goal")
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<string>("Methodology")
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("TrainingId", "Language");

                    b.ToTable("TrainingDetail", (string)null);
                });

            modelBuilder.Entity("Core.Domain.TrainingIdentity", b =>
                {
                    b.Property<int>("TrainingId")
                        .HasColumnType("int");

                    b.Property<int>("TrainingTypeId")
                        .HasColumnType("int");

                    b.HasKey("TrainingId", "TrainingTypeId");

                    b.ToTable("TrainingIdentity", (string)null);
                });

            modelBuilder.Entity("Core.Domain.TrainingSlot", b =>
                {
                    b.Property<int>("TrainingId")
                        .HasColumnType("int");

                    b.Property<int>("TrainingSlotTypeId")
                        .HasColumnType("int");

                    b.HasKey("TrainingId", "TrainingSlotTypeId");

                    b.ToTable("TrainingSlot", (string)null);
                });

            modelBuilder.Entity("Core.Domain.TrainingTarget", b =>
                {
                    b.Property<int>("TrainingId")
                        .HasColumnType("int");

                    b.Property<int>("TrainingTargetAudienceId")
                        .HasColumnType("int");

                    b.HasKey("TrainingId", "TrainingTargetAudienceId");

                    b.ToTable("TrainingTarget", (string)null);
                });

            modelBuilder.Entity("Core.Domain.Trainer", b =>
                {
                    b.OwnsOne("Core.Domain.Name", "Name", b1 =>
                        {
                            b1.Property<int>("TrainerId")
                                .HasColumnType("int");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("nvarchar(200)")
                                .HasColumnName("FirstName");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("nvarchar(200)")
                                .HasColumnName("LastName");

                            b1.HasKey("TrainerId");

                            b1.ToTable("Trainer");

                            b1.WithOwner()
                                .HasForeignKey("TrainerId");
                        });

                    b.OwnsOne("Core.Domain.TrainerIdentity", "Identity", b1 =>
                        {
                            b1.Property<int>("TrainerId")
                                .HasColumnType("int");

                            b1.Property<int>("ApplicationTypeId")
                                .HasMaxLength(200)
                                .HasColumnType("int")
                                .HasColumnName("ApplicationType");

                            b1.Property<string>("UserId")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("nvarchar(200)")
                                .HasColumnName("UserId");

                            b1.HasKey("TrainerId");

                            b1.ToTable("Trainer");

                            b1.WithOwner()
                                .HasForeignKey("TrainerId");
                        });

                    b.Navigation("Identity")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.TrainerAssignment", b =>
                {
                    b.HasOne("Core.Domain.Trainer", "Trainer")
                        .WithMany("Assignments")
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Training", "Training")
                        .WithMany("TrainerAssignments")
                        .HasForeignKey("TrainingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trainer");

                    b.Navigation("Training");
                });

            modelBuilder.Entity("Core.Domain.TrainingDetail", b =>
                {
                    b.HasOne("Core.Domain.Training", "Training")
                        .WithMany("Details")
                        .HasForeignKey("TrainingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Training");
                });

            modelBuilder.Entity("Core.Domain.TrainingIdentity", b =>
                {
                    b.HasOne("Core.Domain.Training", "Training")
                        .WithMany("Identities")
                        .HasForeignKey("TrainingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Training");
                });

            modelBuilder.Entity("Core.Domain.TrainingSlot", b =>
                {
                    b.HasOne("Core.Domain.Training", "Training")
                        .WithMany("Slots")
                        .HasForeignKey("TrainingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Training");
                });

            modelBuilder.Entity("Core.Domain.TrainingTarget", b =>
                {
                    b.HasOne("Core.Domain.Training", "Training")
                        .WithMany("Targets")
                        .HasForeignKey("TrainingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Training");
                });

            modelBuilder.Entity("Core.Domain.Trainer", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("Core.Domain.Training", b =>
                {
                    b.Navigation("Details");

                    b.Navigation("Identities");

                    b.Navigation("Slots");

                    b.Navigation("Targets");

                    b.Navigation("TrainerAssignments");
                });
#pragma warning restore 612, 618
        }
    }
}
