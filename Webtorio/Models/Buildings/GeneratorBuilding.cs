using ErrorOr;
using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Buildings.Services.StateMachine;
using Webtorio.Application.Interfaces;
using Webtorio.Models.StaticData;
using Webtorio.Specifications.Resources;

namespace Webtorio.Models.Buildings;

public class GeneratorBuilding : Building
{
    public GeneratorBuildingType GeneratorBuildingType => (BuildingType as GeneratorBuildingType)!; 
    
    public GeneratorBuilding(string name, BuildingType buildingType)
    {
        Name = name;
        BuildingType = buildingType;
    }

    public override Task<ErrorOr<Success>> UpdateAsync(IRepository repository, BuildingWorkService buildingWorkService, 
        CancellationToken cancellationToken)
    {
        if (BuildingType.Energy == Energy.Burner) 
            BurnerEnergyReserveOnTick -= 1;

        return Task.FromResult<ErrorOr<Success>>(Result.Success);
    }

    public override async Task<CheckResult> CheckConsumptionsAsync(IRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await EnsureEnergyAsync(this, repository, cancellationToken);

        if (!result.IsSuccess)
            return result;

        result = await EnsureGeneratorInputItemsAsync(repository, cancellationToken);

        if (!result.IsSuccess)
            return result;

        return CheckResult.Success();
    }

    private async Task<CheckResult> EnsureGeneratorInputItemsAsync(IRepository repository, 
        CancellationToken cancellationToken)
    {
        if (GeneratorBuildingType.InputItemTypeId is null)
            return CheckResult.Success();

        var result = await repository
            .GetAsync(new ResourceByResourceTypeIdReadOnlySpec(GeneratorBuildingType.InputItemTypeId.Value), 
                cancellationToken);

        if (result.IsError || result.Value.Amount < 0)
            return CheckResult.Failure(BuildingState.NoItems, result.Errors);

        return CheckResult.Success();
    }
    
    public async Task<ErrorOr<Success>> StartGeneratorBuildingAsync(IRepository repository, 
        CancellationToken cancellationToken)
    {
        if (GeneratorBuildingType.InputItemTypeId is not null && GeneratorBuildingType.InputAmount is not null)
        {
            var removeResult = await repository
                .RemoveResourceAsync(GeneratorBuildingType.InputItemTypeId.Value,
                GeneratorBuildingType.InputAmount.Value, cancellationToken);

            if (removeResult.IsError)
                return removeResult.Errors;
        }
        
        var addResult = await repository.AddResourceAsync(GeneratorBuildingType.OutputItemTypeId, 
                GeneratorBuildingType.OutputAmount, cancellationToken);

        if (addResult.IsError)
            return addResult.Errors;

        return Result.Success;
    }
    
    public async Task<ErrorOr<Success>> StopGeneratorBuildingAsync(IRepository repository, 
        CancellationToken cancellationToken)
    {
        if (GeneratorBuildingType.InputItemTypeId is not null && GeneratorBuildingType.InputAmount is not null)
        {
            var addResult = await repository.AddResourceAsync(GeneratorBuildingType.InputItemTypeId.Value,
                GeneratorBuildingType.InputAmount.Value, cancellationToken);

            if (addResult.IsError)
                return addResult.Errors;
        }
        
        var removeResult = await repository.RemoveResourceAsync(GeneratorBuildingType.OutputItemTypeId, 
                GeneratorBuildingType.OutputAmount, cancellationToken, true);

        if (removeResult.IsError)
            return removeResult.Errors;

        return Result.Success;
    }
    
    private GeneratorBuilding(){}
}