using ErrorOr;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Services;

namespace Webtorio.Application.Buildings.Services.StateMachine.States;

public class OnState : IState
{
    public BuildingState BuildingState => BuildingState.On;

    public async Task<ErrorOr<Success>> EnterAsync(GameTickHandler gameTickHandler, IRepository repository, 
        Building building, CancellationToken cancellationToken)
    {
        gameTickHandler.AddBuildingToActiveList(building.Id);
        
        if (building.BuildingType.Energy == Energy.Electric)
        {
            var result = await building.StartUseElectricEnergyAsync(repository, cancellationToken);
            
            if (result.IsError)
                return result.Errors;
        }

        if (building is GeneratorBuilding generatorBuilding)
        {
            var result = await generatorBuilding.StartGeneratorBuildingAsync(repository, cancellationToken);

            if (result.IsError)
                return result.Errors;
        }

        return Result.Success;
    }

    public async Task<ErrorOr<Success>> ExitAsync(GameTickHandler gameTickHandler, IRepository repository, 
        Building building, CancellationToken cancellationToken)
    {
        if (building.BuildingType.Energy == Energy.Electric)
        {
            var result = await building.StopUseElectricEnergyAsync(repository, cancellationToken);
            
            if (result.IsError)
                return result.Errors;
        }
        
        if (building is GeneratorBuilding generatorBuilding)
        {
            var result = await generatorBuilding.StopGeneratorBuildingAsync(repository, cancellationToken);

            if (result.IsError)
                return result.Errors;
        }

        return Result.Success;
    }

    public async Task<CheckResult> CheckConditionsAsync(IRepository repository, Building building,
        CancellationToken cancellationToken) =>
        await building.CheckConsumptionsAsync(repository, cancellationToken);
}