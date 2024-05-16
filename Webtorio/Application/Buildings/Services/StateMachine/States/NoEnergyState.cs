using ErrorOr;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Services;

namespace Webtorio.Application.Buildings.Services.StateMachine.States;

public class NoEnergyState : IState
{
    public BuildingState BuildingState => BuildingState.NoEnergy;

    public async Task<ErrorOr<Success>> EnterAsync(GameTickHandler gameTickHandler, IRepository repository, 
        Building building, CancellationToken cancellationToken)
    {
        gameTickHandler.AddBuildingToActiveList(building.Id);
        return await Task.FromResult(Result.Success);
    }

    public async Task<ErrorOr<Success>> ExitAsync(GameTickHandler gameTickHandler, IRepository repository, 
        Building building, CancellationToken cancellationToken) =>
        await Task.FromResult(Result.Success);
}