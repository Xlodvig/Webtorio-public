using Microsoft.EntityFrameworkCore;
using Webtorio.Models;
using Webtorio.Models.Buildings;
using Webtorio.Models.Recipes;
using Webtorio.Models.StaticData;

namespace Webtorio.PersistentData;

public class ApplicationDbContext : DbContext
{
    public DbSet<Item> Items { get; init; } = null!;
    public DbSet<Resource> Resources { get; init; } = null!;
    public DbSet<Building> Buildings { get; init; } = null!;
    public DbSet<Slot> Slots { get; init; } = null!;
    public DbSet<Deposit> Deposits { get; init; } = null!;
    public DbSet<Recipe> Recipes { get; init; } = null!;
    public DbSet<ItemType> ItemTypes { get; init; } = null!;
    public DbSet<ResourceType> ResourceTypes { get; init; } = null!;
    public DbSet<BuildingType> BuildingTypes { get; init; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        // Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExtractiveBuilding>();
        modelBuilder.Entity<ExtractiveBuildingType>();
        modelBuilder.Entity<DepositSlot>();

        modelBuilder.Entity<ManufactureBuilding>();
        modelBuilder.Entity<ManufactureBuildingType>();

        modelBuilder.Entity<GeneratorBuilding>();
        modelBuilder.Entity<GeneratorBuildingType>();

        modelBuilder.Entity<RecipeItem>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.RecipeItems)
            .HasForeignKey(ri => ri.RecipeId);
        
        modelBuilder.Entity<Recipe>()
            .Ignore(r => r.InputItems)
            .Ignore(r => r.OutputItems);

        modelBuilder.Entity<BuildingRecipeMatch>()
            .HasKey(brm => new { brm.RecipeId, brm.ManufactureBuildingTypeId });
    }
}