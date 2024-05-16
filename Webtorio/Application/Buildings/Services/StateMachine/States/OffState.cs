using ErrorOr;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Services;

namespace Webtorio.Application.Buildings.Services.StateMachine.States;

public class OffState : IState
{
    public BuildingState BuildingState => BuildingState.Off;

    public async Task<ErrorOr<Success>> EnterAsync(GameTickHandler gameTickHandler, IRepository repository, 
        Building building, CancellationToken cancellationToken)
    {
        gameTickHandler.RemoveBuildingFromActiveList(building.Id);
        return await Task.FromResult(Result.Success);
    }

    public async Task<ErrorOr<Success>> ExitAsync(GameTickHandler gameTickHandler, IRepository repository, 
        Building building, CancellationToken cancellationToken) => 
        await Task.FromResult(Result.Success);
}