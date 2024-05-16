using ErrorOr;
using Webtorio.Application.Buildings.Services.StateMachine;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Services;

namespace Webtorio.Application.Buildings.Services;

public class BuildingWorkService
{
    private readonly GameTickHandler _gameTickHandler;
    private readonly IRepository _repository;
    private readonly BuildingStateMachine _stateMachine;

    public BuildingWorkService(GameTickHandler gameTickHandler, IRepository repository, BuildingStateMachine stateMachine)
    {
        _gameTickHandler = gameTickHandler;
        _repository = repository;
        _stateMachine = stateMachine;
    }

    public async Task<ErrorOr<Success>> TurnOnOffAsync(Building building, CancellationToken cancellationToken)
    {
        var targetState = BuildingState.Off;
        
        if (building.State == BuildingState.Off)
            targetState = BuildingState.On;                                     
        
        var result = await _stateMachine.
            ChangeStateAsync(targetState, building, _gameTickHandler, _repository, cancellationToken);

        if (result.IsError)
            return result.Errors;

        if (!result.Value.IsSuccess)
        {
            var onFailureChangingResult = await _stateMachine.
                ChangeStateAsync(result.Value.OnFailureTarget, building, _gameTickHandler, _repository, cancellationToken);

            if (onFailureChangingResult.IsError)
                return onFailureChangingResult.Errors;
        }

        return Result.Success;
    }

    public async Task<ErrorOr<Success>> AddBuildingToSlotAsync(Building building, Slot slot, 
        CancellationToken cancellationToken)
    {
        var result = await _stateMachine.ChangeStateAsync(BuildingState.Off, building,
            _gameTickHandler, _repository, cancellationToken);

        if (!result.IsError)
        {
            slot.BuildingId = building.Id;
            return Result.Success;
        }
        
        if (!result.Value.IsSuccess)
            result.Errors.AddRange(result.Value.Failures!);
        
        return result.Errors;
    }
    
    public async Task<ErrorOr<Success>> StoreBuildingFromSlotAsync(Slot slot, CancellationToken cancellationToken)
    {
        var result = await _stateMachine.ChangeStateAsync(BuildingState.Stored, slot.Building!,
            _gameTickHandler, _repository, cancellationToken);

        if (!result.IsError) 
            return Result.Success;
        
        if (!result.Value.IsSuccess)
            result.Errors.AddRange(result.Value.Failures!);
        
        return result.Errors;
    }

    public async Task<ErrorOr<Success>> StoreRecipeOutputItemsAsync(ManufactureBuilding building, 
        CancellationToken cancellationToken)
    {
        foreach (var recipeOutputItem in building.SelectedRecipe!.OutputItems) 
        {
            var result = await _repository
                .AddResourceAsync(recipeOutputItem.ItemTypeId, recipeOutputItem.ItemAmount, cancellationToken);

            if (result.IsError)
                return result.Errors;
        }

        return Result.Success;
    }
}