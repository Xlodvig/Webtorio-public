using Microsoft.EntityFrameworkCore;
using Webtorio.PersistentData;

namespace Webtorio.StartupFilters;

public class MigrationApplyStartupFilter : IStartupFilter
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MigrationApplyStartupFilter> _logger;

    public MigrationApplyStartupFilter(IServiceScopeFactory serviceScopeFactory, 
        ILogger<MigrationApplyStartupFilter> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        _logger.LogWarning("Pending Migrations: {@Migrations}", 
            context.Database.GetPendingMigrations().ToList());
        _logger.LogWarning("Applied Migrations: {@Migrations}", 
            context.Database.GetAppliedMigrations().ToList());

        context.Database.Migrate();
        return next;
    }
}