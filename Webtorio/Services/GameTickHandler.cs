using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Buildings.Services.StateMachine;
using Webtorio.Application.Interfaces;
using Webtorio.Specifications.Buildings;

namespace Webtorio.Services;

public class GameTickHandler
{
    private readonly List<int> _activeBuildingsIds = new();
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly BuildingStateMachine _buildingStateMachine;
    private readonly ILogger<GameTickHandler> _logger;

    public GameTickHandler(IServiceScopeFactory serviceScopeFactory, BuildingStateMachine buildingStateMachine, 
        ILogger<GameTickHandler> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _buildingStateMachine = buildingStateMachine;
        _logger = logger;
    }

    public void AddBuildingToActiveList(int buildingId)
    {
        if (_activeBuildingsIds.Contains(buildingId))
            return;
        
        _activeBuildingsIds.Add(buildingId);
    }

    public void AddBuildingToActiveList(IEnumerable<int> buildingsIds)
    {
        foreach (var id in buildingsIds) 
            AddBuildingToActiveList(id);
    }

    public void RemoveBuildingFromActiveList(int buildingId) => 
        _activeBuildingsIds.Remove(buildingId);

    public async Task OnGameTickAsync(CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var repository = serviceScope.ServiceProvider.GetRequiredService<IRepository>();
        var buildingWorkService = serviceScope.ServiceProvider.GetRequiredService<BuildingWorkService>();
        
        foreach (var buildingId in _activeBuildingsIds)
        {
            var building = await repository.GetAsync(new BuildingByIdSpec(buildingId), cancellationToken);

            if (building.IsError)
                continue;

            var result = await _buildingStateMachine
                .TickAsync(building.Value, this, repository, buildingWorkService, cancellationToken);

            if (result.IsError) 
                _logger.LogError("Error handling game tick: {ErrorMessages}", 
                    string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}")));
            
            await repository.SaveChangesAsync(cancellationToken);
        }

    }
}