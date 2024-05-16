using ErrorOr;
using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Buildings.Services.StateMachine;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Recipes;
using Webtorio.Models.StaticData;

namespace Webtorio.Models.Buildings;

public class ManufactureBuilding : Building
{
    public double WorkSpeed { get; init; }
    public int? SelectedRecipeId { get; set; }
    public Recipe? SelectedRecipe { get; init; }
    public double? TicksBeforeWorkIsDone { get; set; }

    public ManufactureBuilding(string name, BuildingType buildingType)
    {
        Name = name;
        BuildingType = buildingType;
        WorkSpeed = (buildingType as ManufactureBuildingType)!.WorkSpeed;
    }

    public override async Task<CheckResult> CheckConsumptionsAsync(IRepository repository,
        CancellationToken cancellationToken)
    {
        if (SelectedRecipe is null)
            return CheckResult.Failure(BuildingState.NoRecipe, new List<Error>());

        var result = await EnsureEnergyAsync(this, repository, cancellationToken);

        if (!result.IsSuccess)
            return result;

        result = await EnsureRecipeInputItemsAsync(SelectedRecipe.InputItems, repository, cancellationToken);

        if (!result.IsSuccess)
            return result;

        return CheckResult.Success();
    }
    
    public override async Task<ErrorOr<Success>> UpdateAsync(IRepository repository,
        BuildingWorkService buildingWorkService, CancellationToken cancellationToken)
    {
        TicksBeforeWorkIsDone ??= SelectedRecipe!.CraftingTime / WorkSpeed;

        TicksBeforeWorkIsDone -= 1;

        if (TicksBeforeWorkIsDone <= 0)
        {
            var result = await buildingWorkService
                .StoreRecipeOutputItemsAsync(this, cancellationToken);

            if (result.IsError)
                return result.Errors;

            TicksBeforeWorkIsDone += SelectedRecipe!.CraftingTime / WorkSpeed;
        }

        if (BuildingType.Energy == Energy.Burner)
            BurnerEnergyReserveOnTick -= 1;

        return Result.Success;
    }
    
    private ManufactureBuilding(){}
}