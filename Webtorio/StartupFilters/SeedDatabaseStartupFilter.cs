using Webtorio.Models;
using Webtorio.Models.Buildings;
using Webtorio.Models.Constants;
using Webtorio.Models.Recipes;
using Webtorio.Models.StaticData;
using Webtorio.PersistentData;

namespace Webtorio.StartupFilters;

public class SeedDatabaseStartupFilter : IStartupFilter
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SeedDatabaseStartupFilter(IServiceScopeFactory serviceScopeFactory) => 
        _serviceScopeFactory = serviceScopeFactory;

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var db = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            TrySeedDatabase(db);
        }
        catch
        {
            Console.WriteLine("An error occured while seeding the database");
            throw;
        }

        return next;
    }

    private void TrySeedDatabase(ApplicationDbContext db)
    {
        if (!db.Slots.Any())
            for (var i = 0; i < 18; i++)
                db.Slots.Add(new Slot());

        if (!db.Deposits.Any())
        {
            db.Deposits.AddRange(
                new Deposit(ItemTypeId.Coal, 100000, 18),
                new Deposit(ItemTypeId.Stone, 50000),
                new Deposit(ItemTypeId.CopperOre, 50000),
                new Deposit(ItemTypeId.IronOre, 50000));
        }

        if (!db.ResourceTypes.Any())
        {
            db.ResourceTypes.AddRange(
                new ResourceType { Id = ItemTypeId.Coal, Name = "Coal", IsFuel = true, FuelValueMJ = 4 },
                new ResourceType { Id = ItemTypeId.Stone, Name = "Stone" },
                new ResourceType { Id = ItemTypeId.IronOre, Name = "Iron Ore" },
                new ResourceType { Id = ItemTypeId.CopperOre, Name = "Copper Ore" },
                new ResourceType { Id = ItemTypeId.IronPlate, Name = "Iron Plate" },
                new ResourceType { Id = ItemTypeId.CopperPlate, Name = "Copper Plate" },
                new ResourceType { Id = ItemTypeId.IronStick, Name = "Iron Stick" },
                new ResourceType { Id = ItemTypeId.IronGearWheel, Name = "Iron Gear Wheel" },
                new ResourceType { Id = ItemTypeId.CopperCable, Name = "Copper Cable" },
                new ResourceType { Id = ItemTypeId.CokeCoal, Name = "Coke Coal", IsFuel = true, FuelValueMJ = 10 },
                new ResourceType { Id = ItemTypeId.StoneBrick, Name = "Stone Brick" },
                new ResourceType { Id = ItemTypeId.SteelPlate, Name = "Steel Plate" },
                
                new ResourceType { Id = ItemTypeId.WaterPressure, Name = "Water Pressure", IsNoSolid = true},
                new ResourceType { Id = ItemTypeId.SteamPressure, Name = "Steam Pressure", IsNoSolid = true},
                new ResourceType { Id = ItemTypeId.AvailableElectricity, Name = "Available Electricity", IsNoSolid = true},
                new ResourceType { Id = ItemTypeId.StoredElectricity, Name = "Stored Electricity", IsNoSolid = true });
        }

        if (!db.Recipes.Any())
        {
            db.Recipes.AddRange(new List<Recipe>()
                {
                    new()
                    {
                        Id = RecipeId.IronPlate,
                        Name = "Iron Plate",
                        RecipeItems = new List<RecipeItem>
                        {
                            new() { ItemTypeId = ItemTypeId.IronOre, ItemAmount = 1, RecipeId = RecipeId.IronPlate},
                            
                            new() { ItemTypeId = ItemTypeId.IronPlate, ItemAmount = 1, RecipeId = RecipeId.IronPlate, 
                                    IsOutput = true},
                        },
                        CraftingTime = 3.2,
                        IsAvailable = true,
                    },
                    new()
                    {
                        Id = RecipeId.CopperPlate,
                        Name = "Copper Plate",
                        RecipeItems = new List<RecipeItem>
                        {
                            new() { ItemTypeId = ItemTypeId.CopperOre, ItemAmount = 1, RecipeId = RecipeId.CopperPlate },
                      
                            new() { ItemTypeId = ItemTypeId.CopperPlate, ItemAmount = 1, RecipeId = RecipeId.CopperPlate, 
                                    IsOutput = true },
                        },
                        CraftingTime = 3.2,
                        IsAvailable = true,
                    },
                    new()
                    {
                        Id = RecipeId.StoneBrick,
                        Name = "Stone Brick",
                        RecipeItems = new List<RecipeItem>
                        {
                            new() { ItemTypeId = ItemTypeId.Stone, ItemAmount = 2, RecipeId = RecipeId.StoneBrick },
                      
                            new() { ItemTypeId = ItemTypeId.StoneBrick, ItemAmount = 1, RecipeId = RecipeId.StoneBrick,
                                    IsOutput = true },
                        },
                        CraftingTime = 3.2,
                        IsAvailable = true,
                    },
                    new()
                    {
                        Id = RecipeId.SteelPlate,
                        Name = "Steel Plate",
                        RecipeItems = new List<RecipeItem>
                        {
                            new() { ItemTypeId = ItemTypeId.IronPlate, ItemAmount = 5, RecipeId = RecipeId.SteelPlate },
                      
                            new() { ItemTypeId = ItemTypeId.SteelPlate, ItemAmount = 1, RecipeId = RecipeId.SteelPlate,
                                    IsOutput = true },
                        },
                        CraftingTime = 16,
                        IsAvailable = true,
                    },
                    new()
                    {
                        Id = RecipeId.CokeCoal,
                        Name = "Coke Coal",
                        RecipeItems = new List<RecipeItem>
                        {
                            new() { ItemTypeId = ItemTypeId.Coal, ItemAmount = 2, RecipeId = RecipeId.CokeCoal },
                            
                            new() { ItemTypeId = ItemTypeId.CokeCoal, ItemAmount = 1, RecipeId = RecipeId.CokeCoal, 
                                    IsOutput = true },
                        },
                        CraftingTime = 5,
                        IsAvailable = true,
                    },
                });
        }
        
        if (!db.BuildingTypes.Any())
        {
            db.BuildingTypes.AddRange(
                new ExtractiveBuildingType
                {
                    Id = ItemTypeId.BurnerMiningDrill,
                    Name = "Burner Mining Drill",
                    MiningSpeed = 0.25,
                    Energy = Energy.Burner,
                    EnergyConsumptionKW = 150,
                    IsAvailable = true,
                },

                new ExtractiveBuildingType
                {
                    Id = ItemTypeId.ElectricMiningDrill,
                    Name = "Electric Mining Drill",
                    MiningSpeed = 0.5,
                    Energy = Energy.Electric,
                    EnergyConsumptionKW = 90,
                    IsAvailable = true,
                },
                
                new GeneratorBuildingType
                {
                    Id = ItemTypeId.Boiler,
                    Name = "Boiler",
                    Energy = Energy.Burner,
                    EnergyConsumptionKW = 1800,
                    InputItemTypeId = ItemTypeId.WaterPressure,
                    InputAmount = 60,
                    OutputItemTypeId = ItemTypeId.SteamPressure,
                    OutputAmount = 60,
                    IsAvailable = true,
                },
                
                new GeneratorBuildingType
                {
                    Id = ItemTypeId.SteamEngine,
                    Name = "Steam Engine",
                    Energy = Energy.NoEnergy,
                    InputItemTypeId = ItemTypeId.SteamPressure,
                    InputAmount = 30,
                    OutputItemTypeId = ItemTypeId.AvailableElectricity,
                    OutputAmount = 900,
                    IsAvailable = true,
                },
                
                new GeneratorBuildingType
                {
                  Id  = ItemTypeId.OffshorePump,
                  Name = "Offshore Pump",
                  Energy = Energy.NoEnergy,
                  OutputItemTypeId = ItemTypeId.WaterPressure,
                  OutputAmount = 1200,
                  IsAvailable = true,
                },

                new ManufactureBuildingType
                {
                    Id = ItemTypeId.StoneFurnace,
                    Name = "Stone Furnace",
                    Energy = Energy.Burner,
                    EnergyConsumptionKW = 90,
                    WorkSpeed = 1,
                    BuildingRecipeMatches = new List<BuildingRecipeMatch>
                    {
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.StoneFurnace,
                            RecipeId = RecipeId.IronPlate,
                        }, 
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.StoneFurnace,
                            RecipeId = RecipeId.CopperPlate,
                        },
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.StoneFurnace,
                            RecipeId = RecipeId.StoneBrick,
                        },
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.StoneFurnace,
                            RecipeId = RecipeId.SteelPlate,
                        },
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.StoneFurnace,
                            RecipeId = RecipeId.CokeCoal,
                        }, 
                    },
                    IsAvailable = true, 
                },
                
                new ManufactureBuildingType
                {
                    Id = ItemTypeId.SteelFurnace,
                    Name = "Steel Furnace",
                    Energy = Energy.Burner,
                    EnergyConsumptionKW = 90,
                    WorkSpeed = 2,
                    BuildingRecipeMatches = new List<BuildingRecipeMatch>
                    {
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.SteelFurnace,
                            RecipeId = RecipeId.IronPlate,
                        }, 
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.SteelFurnace,
                            RecipeId = RecipeId.CopperPlate,
                        },
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.SteelFurnace,
                            RecipeId = RecipeId.StoneBrick,
                        },
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.SteelFurnace,
                            RecipeId = RecipeId.SteelPlate,
                        },
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.SteelFurnace,
                            RecipeId = RecipeId.CokeCoal,
                        }, 
                    },
                    IsAvailable = true, 
                },
                
                new ManufactureBuildingType
                {
                    Id = ItemTypeId.ElectricFurnace,
                    Name = "Electric Furnace",
                    Energy = Energy.Electric,
                    EnergyConsumptionKW = 180,
                    WorkSpeed = 2,
                    BuildingRecipeMatches = new List<BuildingRecipeMatch>
                    {
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.ElectricFurnace,
                            RecipeId = RecipeId.IronPlate,
                        }, 
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.ElectricFurnace,
                            RecipeId = RecipeId.CopperPlate,
                        },
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.ElectricFurnace,
                            RecipeId = RecipeId.StoneBrick,
                        },
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.ElectricFurnace,
                            RecipeId = RecipeId.SteelPlate,
                        },
                        new()
                        {
                            ManufactureBuildingTypeId = ItemTypeId.ElectricFurnace,
                            RecipeId = RecipeId.CokeCoal,
                        }, 
                    },
                    IsAvailable = true, 
                }
            );
        }
        
        db.SaveChanges();
    }
}