using Webtorio.Models.Buildings;
using Webtorio.PersistentData;
using Webtorio.Services;

namespace Webtorio.StartupFilters;

public class LoadBuildingStateStartupFilter : IStartupFilter
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly GameTickHandler _gameTickHandler;

    public LoadBuildingStateStartupFilter(IServiceScopeFactory serviceScopeFactory, GameTickHandler gameTickHandler)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _gameTickHandler = gameTickHandler;
    }
    
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var db = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var activeBuildingsIds = db.Buildings
            .Where(b => 
                b.State == BuildingState.On ||
                b.State == BuildingState.NoItems ||
                b.State == BuildingState.NoEnergy ||
                b.State == BuildingState.NoRecipe)
            .Select(b => b.Id)
            .ToList();
        
        _gameTickHandler.AddBuildingToActiveList(activeBuildingsIds);

        return next;
    }
}