using ErrorOr;
using Webtorio.Application.Buildings.Services.StateMachine.States;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Errors;
using Webtorio.Models.Buildings;
using Webtorio.Services;

namespace Webtorio.Application.Buildings.Services.StateMachine;

public class BuildingStateMachine
{
    private readonly Dictionary<BuildingState, IState> _extractiveStates = new()
    {
        { BuildingState.On, new OnState() },
        { BuildingState.Off, new OffState() },
        { BuildingState.NoEnergy, new NoEnergyState() },
        { BuildingState.Stored, new StoredState() },
    };
    
    private readonly Dictionary<BuildingState, IState> _manufactureStates = new()
    {
        { BuildingState.On, new OnState() },
        { BuildingState.Off, new OffState() },
        { BuildingState.NoEnergy, new NoEnergyState() },            
        { BuildingState.NoItems, new NoItemsState() },
        { BuildingState.NoRecipe, new NoRecipeState() },
        { BuildingState.Stored, new StoredState() },
    };
    
    private readonly Dictionary<BuildingState, IState> _generatorStates = new()
    {
        { BuildingState.On, new OnState() },
        { BuildingState.Off, new OffState() },
        { BuildingState.NoEnergy, new NoEnergyState() },            
        { BuildingState.NoItems, new NoItemsState() },
        { BuildingState.Stored, new StoredState() },
    };

    public async Task<ErrorOr<CheckResult>> ChangeStateAsync(BuildingState target, Building building, 
        GameTickHandler gameTickHandler, IRepository repository, CancellationToken cancellationToken)
    {
        IState? currentState = GetStateBy(building.State, building);
        
        if (currentState == null)
            return Errors.BuildingWork.ErrorGettingState;

        IState? targetState = GetStateBy(target, building);
        
        if (targetState == null)
            return Errors.BuildingWork.ErrorGettingState;
        
        var checkResult = await targetState.CheckConditionsAsync(repository, building, cancellationToken);

        if (!checkResult.IsSuccess)
            return checkResult;

        var result = await currentState.ExitAsync(gameTickHandler, repository, building, cancellationToken);

        if (result.IsError)
            return result.Errors;
        
        result = await targetState.EnterAsync(gameTickHandler, repository, building, cancellationToken);

        if (result.IsError)
            return result.Errors;
        
        building.State = target;

        return CheckResult.Success();
    }

    public async Task<ErrorOr<Success>> TickAsync(Building building, GameTickHandler gameTickHandler,
        IRepository repository, BuildingWorkService buildingWorkService, CancellationToken cancellationToken)
    {
        var checkResult = await building.CheckConsumptionsAsync(repository, cancellationToken);

        switch (checkResult.IsSuccess)
        {
            case false:
                if (building.State == checkResult.OnFailureTarget) 
                    return checkResult.Failures!;
                
                var changeResult = await ChangeStateAsync(checkResult.OnFailureTarget, building, 
                    gameTickHandler, repository, cancellationToken);
                    
                if (changeResult.IsError) 
                    checkResult.Failures!.AddRange(changeResult.Errors);

                return checkResult.Failures!;
            
            case true when building.State != BuildingState.On:
                var result = 
                    await ChangeStateAsync(BuildingState.On, building, gameTickHandler, repository, 
                        cancellationToken);

                if (result.IsError)
                    return result.Errors;
                
                break;
        }

        return await building.UpdateAsync(repository, buildingWorkService, cancellationToken);
    }

    private IState? GetStateBy(BuildingState buildingState, Building building)
    {
        return building switch
        {
            ExtractiveBuilding => _extractiveStates[buildingState],
            ManufactureBuilding => _manufactureStates[buildingState],
            GeneratorBuilding => _generatorStates[buildingState],
            _ => null,
        };
    }
}