﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Webtorio.PersistentData;

#nullable disable

namespace Webtorio.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230329214709_RemoveTitleFromBuildingSlot")]
    partial class RemoveTitleFromBuildingSlot
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Webtorio.Models.Buildings.BuildingSlot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BuildingId")
                        .HasColumnType("integer");

                    b.Property<int?>("DepositId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId")
                        .IsUnique();

                    b.HasIndex("DepositId");

                    b.ToTable("BuildingSlots");
                });

            modelBuilder.Entity("Webtorio.Models.Deposit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("ResourceAmount")
                        .HasColumnType("double precision");

                    b.Property<int>("ResourceTypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResourceTypeId");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("Webtorio.Models.ItemsBase.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Items");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Item");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.BuildingType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BuildingTypes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BuildingType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.ResourceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ResourceTypes");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.Building", b =>
                {
                    b.HasBaseType("Webtorio.Models.ItemsBase.Item");

                    b.Property<int>("BuildingTypeId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsStored")
                        .HasColumnType("boolean");

                    b.Property<int>("WorkStatus")
                        .HasColumnType("integer");

                    b.HasIndex("BuildingTypeId");

                    b.HasDiscriminator().HasValue("Building");
                });

            modelBuilder.Entity("Webtorio.Models.Resources.Resource", b =>
                {
                    b.HasBaseType("Webtorio.Models.ItemsBase.Item");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<int>("ResourceTypeId")
                        .HasColumnType("integer");

                    b.HasIndex("ResourceTypeId");

                    b.HasDiscriminator().HasValue("Resource");
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.ExtractiveBuildingType", b =>
                {
                    b.HasBaseType("Webtorio.Models.StaticData.BuildingType");

                    b.Property<double>("MiningSpeed")
                        .HasColumnType("double precision");

                    b.HasDiscriminator().HasValue("ExtractiveBuildingType");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.ExtractiveBuilding", b =>
                {
                    b.HasBaseType("Webtorio.Models.Buildings.Building");

                    b.Property<double>("MiningSpeed")
                        .HasColumnType("double precision");

                    b.HasDiscriminator().HasValue("ExtractiveBuilding");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.BuildingSlot", b =>
                {
                    b.HasOne("Webtorio.Models.Buildings.Building", "Building")
                        .WithOne("BuildingSlot")
                        .HasForeignKey("Webtorio.Models.Buildings.BuildingSlot", "BuildingId");

                    b.HasOne("Webtorio.Models.Deposit", "Deposit")
                        .WithMany("BuildingSlots")
                        .HasForeignKey("DepositId");

                    b.Navigation("Building");

                    b.Navigation("Deposit");
                });

            modelBuilder.Entity("Webtorio.Models.Deposit", b =>
                {
                    b.HasOne("Webtorio.Models.StaticData.ResourceType", "ResourceType")
                        .WithMany()
                        .HasForeignKey("ResourceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ResourceType");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.Building", b =>
                {
                    b.HasOne("Webtorio.Models.StaticData.BuildingType", "BuildingType")
                        .WithMany()
                        .HasForeignKey("BuildingTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BuildingType");
                });

            modelBuilder.Entity("Webtorio.Models.Resources.Resource", b =>
                {
                    b.HasOne("Webtorio.Models.StaticData.ResourceType", "ResourceType")
                        .WithMany()
                        .HasForeignKey("ResourceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ResourceType");
                });

            modelBuilder.Entity("Webtorio.Models.Deposit", b =>
                {
                    b.Navigation("BuildingSlots");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.Building", b =>
                {
                    b.Navigation("BuildingSlot");
                });
#pragma warning restore 612, 618
        }
    }
}
