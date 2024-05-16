using ErrorOr;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Services;

namespace Webtorio.Application.Buildings.Services.StateMachine.States;

public class StoredState : IState
{
    public BuildingState BuildingState => BuildingState.Stored;

    public async Task<ErrorOr<Success>> EnterAsync(GameTickHandler gameTickHandler, IRepository repository, 
        Building building, CancellationToken cancellationToken)
    {
        gameTickHandler.RemoveBuildingFromActiveList(building.Id);
        
        if (building.BuildingType.Energy == Energy.Burner) 
            building.BurnerEnergyReserveOnTick = null;

        switch (building)
        {
            case ExtractiveBuilding extractiveBuilding:
                extractiveBuilding.ResourceBuffer = 0;
                break;
            
            case ManufactureBuilding productiveBuilding:
                productiveBuilding.TicksBeforeWorkIsDone = null;
                productiveBuilding.SelectedRecipeId = null;
                break;
        }
        
        if (building.Slot != null) 
            building.Slot.BuildingId = null;

        return await Task.FromResult(Result.Success);
    }

    public async Task<ErrorOr<Success>> ExitAsync(GameTickHandler gameTickHandler, IRepository repository, 
        Building building, CancellationToken cancellationToken) => 
        await Task.FromResult(Result.Success);
}