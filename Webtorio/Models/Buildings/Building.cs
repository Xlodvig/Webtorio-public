using ErrorOr;
using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Buildings.Services.StateMachine;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Errors;
using Webtorio.Models.Constants;
using Webtorio.Models.Recipes;
using Webtorio.Models.StaticData;
using Webtorio.Specifications.Resources;

namespace Webtorio.Models.Buildings;

public abstract class Building : Item
{
    public BuildingState State { get; set; } = BuildingState.Stored;
    public int BuildingTypeId { get; init; }
    public BuildingType BuildingType { get; init; } = null!;
    public double? BurnerEnergyReserveOnTick { get; set; }
    public Slot? Slot { get; init; }
    public int? SelectedFuelResourceTypeId { get; set; }
    public ResourceType? SelectedFuelResourceType { get; init; }

    public virtual Task<CheckResult> CheckConsumptionsAsync(IRepository repository, CancellationToken cancellationToken) =>
        Task.FromResult(CheckResult.Success());

    public virtual Task<ErrorOr<Success>> UpdateAsync(IRepository repository, BuildingWorkService buildingWorkService,
        CancellationToken cancellationToken) =>
        Task.FromResult<ErrorOr<Success>>(Result.Success);

    protected static async Task<CheckResult> EnsureEnergyAsync(Building building, IRepository repository,
        CancellationToken cancellationToken)
    {
        switch (building.BuildingType.Energy)
        {
            case Energy.Burner:
                if (building.BurnerEnergyReserveOnTick is null or < 1)
                {
                    var selectedFuelResourceTypeId = building.SelectedFuelResourceTypeId!.Value;

                    var result = await repository
                        .RemoveResourceAsync(selectedFuelResourceTypeId, 1, cancellationToken);

                    if (result.IsError)
                        return CheckResult.Failure(BuildingState.NoEnergy,
                            result.Errors);

                    var ticksByFuelTypeAndBuildingConsumption =
                        (building.SelectedFuelResourceType!.FuelValueMJ /
                         (building.BuildingType.EnergyConsumptionKW / 1000.0))!.Value;

                    building.BurnerEnergyReserveOnTick += ticksByFuelTypeAndBuildingConsumption;
                    building.BurnerEnergyReserveOnTick ??= ticksByFuelTypeAndBuildingConsumption;
                }

                return CheckResult.Success();

            case Energy.Electric:

                var resourceResult = await repository
                    .GetAsync(new ResourceByResourceTypeIdSpec(ItemTypeId.AvailableElectricity), cancellationToken);

                if (resourceResult.IsError)
                    return CheckResult.Failure(BuildingState.NoEnergy, resourceResult.Errors);

                if (InsufficientElectricityToStartOrContinueBuildingWork(building, resourceResult))
                    return CheckResult.Failure(BuildingState.NoEnergy,
                        new List<Error> { Errors.Resource.AmountInsufficiency(resourceResult.Value.ResourceTypeId) });

                return CheckResult.Success();

            case Energy.NoEnergy:
            default:
                return CheckResult.Success();
        }
    }

    public async Task<ErrorOr<Success>> StartUseElectricEnergyAsync(IRepository repository, 
        CancellationToken cancellationToken)
    {
        var result = await repository.RemoveResourceAsync(ItemTypeId.AvailableElectricity,
            BuildingType.EnergyConsumptionKW, cancellationToken);
        
        if (result.IsError)
            return result.Errors;

        return Result.Success;
    }

    public async Task<ErrorOr<Success>> StopUseElectricEnergyAsync(IRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.AddResourceAsync(ItemTypeId.AvailableElectricity,
            BuildingType.EnergyConsumptionKW, cancellationToken);
        
        if (result.IsError)
            return result.Errors;

        return Result.Success;
    }

    protected async Task<CheckResult> EnsureRecipeInputItemsAsync(IEnumerable<RecipeItem> items,
        IRepository repository, CancellationToken cancellationToken)
    {
        foreach (var item in items)
        {
            if (item is not { IsOutput: false, IsBufferEmpty: true })
                continue;

            var resourceResult =
                await repository.RemoveResourceAsync(item.ItemTypeId, item.ItemAmount, cancellationToken);

            if (resourceResult.IsError)
                return CheckResult.Failure(BuildingState.NoItems, resourceResult.Errors);

            item.IsBufferEmpty = false;
        }

        return CheckResult.Success();
    }

    private static bool InsufficientElectricityToStartOrContinueBuildingWork(Building building,
        ErrorOr<Resource> resourceResult) =>
        (resourceResult.Value.Amount < 0 && building.State == BuildingState.On) || 
        (resourceResult.Value.Amount - building.BuildingType.EnergyConsumptionKW < 0 && 
         building.State != BuildingState.On);
}