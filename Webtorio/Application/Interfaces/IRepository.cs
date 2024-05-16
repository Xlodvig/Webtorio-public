using Ardalis.Specification;
using ErrorOr;
using Webtorio.Models.Buildings;

namespace Webtorio.Application.Interfaces;

public interface IRepository
{
    public Task<ErrorOr<TEntity>> GetAsync<TEntity>(ISpecification<TEntity> spec, CancellationToken cancellationToken)
        where TEntity : class, IEntity<int>;

    public Task<List<TEntity>> GetAllAsync<TEntity>(ISpecification<TEntity> spec, CancellationToken cancellationToken)
        where TEntity : class, IEntity<int>;

    public Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : class, IEntity<int>;

    public Task<ErrorOr<int>> AddResourceAsync(int resourceTypeId, double amount, CancellationToken cancellationToken);

    public Task<List<TEntity>> GetAllFirstByBuildingTypeAsync<TEntity>(ISpecification<TEntity> spec,
        CancellationToken cancellationToken) where TEntity : Building, IEntity<int>;

    public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity<int>;

    public Task<ErrorOr<Success>> RemoveResourceAsync(int resourceTypeId, double amount, 
        CancellationToken cancellationToken, bool isStopGeneratorBuilding = false);

    public Task SaveChangesAsync(CancellationToken cancellationToken);
}