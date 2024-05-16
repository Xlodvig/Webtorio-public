using ErrorOr;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Services;

namespace Webtorio.Application.Buildings.Services.StateMachine;

public interface IState
{
    public BuildingState BuildingState { get; }

    public Task<ErrorOr<Success>> EnterAsync(GameTickHandler gameTickHandler, IRepository repository, Building building, 
        CancellationToken cancellationToken);
    
    public Task<ErrorOr<Success>> ExitAsync(GameTickHandler gameTickHandler, IRepository repository, Building building, 
        CancellationToken cancellationToken);

    public async Task<CheckResult> CheckConditionsAsync(IRepository repository, Building building,
        CancellationToken cancellationToken) =>
        await Task.FromResult(CheckResult.Success());
}