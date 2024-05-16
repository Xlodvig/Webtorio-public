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
    [Migration("20231129144527_RemoveIsStored")]
    partial class RemoveIsStored
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Webtorio.Models.Buildings.Slot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BuildingId")
                        .HasColumnType("integer");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId")
                        .IsUnique();

                    b.ToTable("Slots");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Slot");

                    b.UseTphMappingStrategy();
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

            modelBuilder.Entity("Webtorio.Models.Recipes.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("CraftingTime")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Webtorio.Models.Recipes.RecipeItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsBufferEmpty")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOutput")
                        .HasColumnType("boolean");

                    b.Property<double>("ItemAmount")
                        .HasColumnType("double precision");

                    b.Property<int>("ItemTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("RecipeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ItemTypeId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeItem");
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.BuildingRecipeMatch", b =>
                {
                    b.Property<int>("RecipeId")
                        .HasColumnType("integer");

                    b.Property<int>("ManufactureBuildingTypeId")
                        .HasColumnType("integer");

                    b.HasKey("RecipeId", "ManufactureBuildingTypeId");

                    b.HasIndex("ManufactureBuildingTypeId");

                    b.ToTable("BuildingRecipeMatch");
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.ItemType", b =>
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

                    b.ToTable("ItemTypes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ItemType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.DepositSlot", b =>
                {
                    b.HasBaseType("Webtorio.Models.Buildings.Slot");

                    b.Property<int>("DepositId")
                        .HasColumnType("integer");

                    b.HasIndex("DepositId");

                    b.HasDiscriminator().HasValue("DepositSlot");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.Building", b =>
                {
                    b.HasBaseType("Webtorio.Models.ItemsBase.Item");

                    b.Property<int>("BuildingTypeId")
                        .HasColumnType("integer");

                    b.Property<double?>("BurnerEnergyReserveOnTick")
                        .HasColumnType("double precision");

                    b.Property<int?>("SelectedFuelResourceTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasIndex("BuildingTypeId");

                    b.HasIndex("SelectedFuelResourceTypeId");

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

            modelBuilder.Entity("Webtorio.Models.StaticData.BuildingType", b =>
                {
                    b.HasBaseType("Webtorio.Models.StaticData.ItemType");

                    b.Property<int>("Energy")
                        .HasColumnType("integer");

                    b.Property<int>("EnergyConsumptionKW")
                        .HasColumnType("integer");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean");

                    b.HasDiscriminator().HasValue("BuildingType");
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.ResourceType", b =>
                {
                    b.HasBaseType("Webtorio.Models.StaticData.ItemType");

                    b.Property<double?>("FuelValueMJ")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsFuel")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsNoSolid")
                        .HasColumnType("boolean");

                    b.HasDiscriminator().HasValue("ResourceType");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.ExtractiveBuilding", b =>
                {
                    b.HasBaseType("Webtorio.Models.Buildings.Building");

                    b.Property<double>("MiningSpeed")
                        .HasColumnType("double precision");

                    b.Property<double>("ResourceBuffer")
                        .HasColumnType("double precision");

                    b.HasDiscriminator().HasValue("ExtractiveBuilding");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.GeneratorBuilding", b =>
                {
                    b.HasBaseType("Webtorio.Models.Buildings.Building");

                    b.HasDiscriminator().HasValue("GeneratorBuilding");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.ManufactureBuilding", b =>
                {
                    b.HasBaseType("Webtorio.Models.Buildings.Building");

                    b.Property<int?>("SelectedRecipeId")
                        .HasColumnType("integer");

                    b.Property<double?>("TicksBeforeWorkIsDone")
                        .HasColumnType("double precision");

                    b.Property<double>("WorkSpeed")
                        .HasColumnType("double precision");

                    b.HasIndex("SelectedRecipeId");

                    b.HasDiscriminator().HasValue("ManufactureBuilding");
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.ExtractiveBuildingType", b =>
                {
                    b.HasBaseType("Webtorio.Models.StaticData.BuildingType");

                    b.Property<double>("MiningSpeed")
                        .HasColumnType("double precision");

                    b.HasDiscriminator().HasValue("ExtractiveBuildingType");
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.GeneratorBuildingType", b =>
                {
                    b.HasBaseType("Webtorio.Models.StaticData.BuildingType");

                    b.Property<double?>("InputAmount")
                        .HasColumnType("double precision");

                    b.Property<int?>("InputItemTypeId")
                        .HasColumnType("integer");

                    b.Property<double>("OutputAmount")
                        .HasColumnType("double precision");

                    b.Property<int>("OutputItemTypeId")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("GeneratorBuildingType");
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.ManufactureBuildingType", b =>
                {
                    b.HasBaseType("Webtorio.Models.StaticData.BuildingType");

                    b.Property<double>("WorkSpeed")
                        .HasColumnType("double precision");

                    b.HasDiscriminator().HasValue("ManufactureBuildingType");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.Slot", b =>
                {
                    b.HasOne("Webtorio.Models.Buildings.Building", "Building")
                        .WithOne("Slot")
                        .HasForeignKey("Webtorio.Models.Buildings.Slot", "BuildingId");

                    b.Navigation("Building");
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

            modelBuilder.Entity("Webtorio.Models.Recipes.RecipeItem", b =>
                {
                    b.HasOne("Webtorio.Models.StaticData.ItemType", "ItemType")
                        .WithMany()
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webtorio.Models.Recipes.Recipe", "Recipe")
                        .WithMany("RecipeItems")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemType");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.BuildingRecipeMatch", b =>
                {
                    b.HasOne("Webtorio.Models.StaticData.ManufactureBuildingType", "ManufactureBuildingType")
                        .WithMany("BuildingRecipeMatches")
                        .HasForeignKey("ManufactureBuildingTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webtorio.Models.Recipes.Recipe", "Recipe")
                        .WithMany("BuildingRecipeMatches")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ManufactureBuildingType");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.DepositSlot", b =>
                {
                    b.HasOne("Webtorio.Models.Deposit", "Deposit")
                        .WithMany("ExtractiveSlots")
                        .HasForeignKey("DepositId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Deposit");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.Building", b =>
                {
                    b.HasOne("Webtorio.Models.StaticData.BuildingType", "BuildingType")
                        .WithMany()
                        .HasForeignKey("BuildingTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webtorio.Models.StaticData.ResourceType", "SelectedFuelResourceType")
                        .WithMany()
                        .HasForeignKey("SelectedFuelResourceTypeId");

                    b.Navigation("BuildingType");

                    b.Navigation("SelectedFuelResourceType");
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

            modelBuilder.Entity("Webtorio.Models.Buildings.ManufactureBuilding", b =>
                {
                    b.HasOne("Webtorio.Models.Recipes.Recipe", "SelectedRecipe")
                        .WithMany()
                        .HasForeignKey("SelectedRecipeId");

                    b.Navigation("SelectedRecipe");
                });

            modelBuilder.Entity("Webtorio.Models.Deposit", b =>
                {
                    b.Navigation("ExtractiveSlots");
                });

            modelBuilder.Entity("Webtorio.Models.Recipes.Recipe", b =>
                {
                    b.Navigation("BuildingRecipeMatches");

                    b.Navigation("RecipeItems");
                });

            modelBuilder.Entity("Webtorio.Models.Buildings.Building", b =>
                {
                    b.Navigation("Slot");
                });

            modelBuilder.Entity("Webtorio.Models.StaticData.ManufactureBuildingType", b =>
                {
                    b.Navigation("BuildingRecipeMatches");
                });
#pragma warning restore 612, 618
        }
    }
}
