﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;

#nullable disable

namespace Smart.FA.Catalog.Showcase.Infrastructure.Migrations
{
    [DbContext(typeof(CatalogShowcaseContext))]
    [Migration("20220711104431_empty")]
    partial class empty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Smart.FA.Catalog.Showcase.Domain.Models.TrainerDetails", b =>
                {
                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<string>("Email")
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ProfileImagePath")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("SocialNetwork")
                        .HasColumnType("int")
                        .HasColumnName("SocialNetworkId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("UrlToProfile")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.ToView("v_TrainerDetails", "Cfa");
                });

            modelBuilder.Entity("Smart.FA.Catalog.Showcase.Domain.Models.TrainerList", b =>
                {
                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ProfileImagePath")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int?>("TrainingCount")
                        .HasColumnType("int");

                    b.ToView("v_TrainerList", "Cfa");
                });

            modelBuilder.Entity("Smart.FA.Catalog.Showcase.Domain.Models.TrainingDetails", b =>
                {
                    b.Property<string>("Goal")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nchar(2)")
                        .IsFixedLength();

                    b.Property<string>("Methodology")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("PracticalModalities")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("ProfileImagePath")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int")
                        .HasColumnName("TrainingStatusTypeId");

                    b.Property<string>("TrainerFirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("FirstName");

                    b.Property<int>("TrainerId")
                        .HasColumnType("int");

                    b.Property<string>("TrainerLastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("LastName");

                    b.Property<string>("TrainerTitle")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("trainerTitle");

                    b.Property<string>("TrainingTitle")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("Title");

                    b.Property<int>("TrainingTopicId")
                        .HasColumnType("int")
                        .HasColumnName("TopicId");

                    b.ToView("v_TrainingDetails", "Cfa");
                });

            modelBuilder.Entity("Smart.FA.Catalog.Showcase.Domain.Models.TrainingList", b =>
                {
                    b.Property<string>("Goal")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nchar(2)")
                        .IsFixedLength();

                    b.Property<string>("Methodology")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("TrainingStatusTypeId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Topic")
                        .HasColumnType("int")
                        .HasColumnName("TopicId");

                    b.Property<string>("TrainerFirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("FirstName");

                    b.Property<int>("TrainerId")
                        .HasColumnType("int");

                    b.Property<string>("TrainerLastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("LastName");

                    b.ToView("v_TrainingList", "Cfa");
                });
#pragma warning restore 612, 618
        }
    }
}
