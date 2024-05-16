using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Errors;
using Webtorio.Models;
using Webtorio.Models.Buildings;
using Webtorio.Specifications.Resources;

namespace Webtorio.PersistentData.Repositories;

public class Repository : IRepository
{
    private readonly ApplicationDbContext _db;

    public Repository(ApplicationDbContext db) => 
        _db = db;

    public async Task<ErrorOr<TEntity>> GetAsync<TEntity>(ISpecification<TEntity> spec, CancellationToken cancellationToken) 
        where TEntity : class, IEntity<int>
    {
        var entity = await SpecificationEvaluator.Default
            .GetQuery(
                query: _db.Set<TEntity>().AsQueryable(),
                specification: spec)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity is null)
            return Errors.Common.NotFound<TEntity>();

        return entity;
    }

    public async Task<List<TEntity>> GetAllAsync<TEntity>(ISpecification<TEntity> spec, CancellationToken cancellationToken)
        where TEntity : class, IEntity<int>
    {
        return await SpecificationEvaluator.Default
            .GetQuery(
                query: _db.Set<TEntity>().AsQueryable(),
                specification: spec)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : class, IEntity<int> =>
        await _db.Set<TEntity>().AddAsync(entity, cancellationToken);

    public async Task<ErrorOr<int>> AddResourceAsync(int resourceTypeId, double amount, CancellationToken cancellationToken)
    {
        var resourceType = await _db.ResourceTypes
            .SingleOrDefaultAsync(rd => rd.Id == resourceTypeId, cancellationToken);
        
        if (resourceType is null)
            return Errors.Resource.TypeNotFound;

        var resourceResult = 
            await GetAsync(new ResourceByResourceTypeIdSpec(resourceTypeId), cancellationToken);
        
        Resource resource;
        
        if (resourceResult.IsError)
        {
            resource = new Resource
            {
                Amount = amount,
                ResourceType = resourceType,
                Name = resourceType.Name,
            };

            await AddAsync(resource, cancellationToken);
        }
        else
        {
            resource = resourceResult.Value;
            resource.Amount += amount;
        }

        return resource.Id;
    }

    public async Task<List<TEntity>> GetAllFirstByBuildingTypeAsync<TEntity>(ISpecification<TEntity> spec, CancellationToken cancellationToken) 
        where TEntity : Building, IEntity<int>
    {
        return await SpecificationEvaluator.Default
            .GetQuery(
                query: _db.Buildings.OfType<TEntity>().AsQueryable(),
                specification: spec)
            .GroupBy(building => building.BuildingTypeId)
            .Select(g => g.First())
            .ToListAsync(cancellationToken);
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity<int> => 
        _db.Set<TEntity>().Remove(entity);

    public async Task<ErrorOr<Success>> RemoveResourceAsync(int resourceTypeId, double amount, 
        CancellationToken cancellationToken, bool isStopGeneratorBuilding = false) => 
        await EnsureResourceAsync(resourceTypeId, amount, isStopGeneratorBuilding, cancellationToken);
    
    private async Task<ErrorOr<Success>> EnsureResourceAsync(int resourceTypeId, double amount, 
        bool isStopGeneratorBuilding, CancellationToken cancellationToken)
    {
        var resourceResult = 
            await GetAsync(new ResourceWithResourceTypeByResourceTypeIdSpec(resourceTypeId), cancellationToken);

        if (resourceResult.IsError)
            return resourceResult.Errors;

        if (resourceResult.Value.Amount - amount < 0 
            && (!isStopGeneratorBuilding || !resourceResult.Value.ResourceType.IsNoSolid)) 
            return Errors.Resource.AmountInsufficiency(resourceTypeId);

        RemoveResource(resourceResult.Value, amount);

        return Result.Success;
    }

    private void RemoveResource(Resource resource, double amount)
    {
        resource.Amount -= amount;

        if (resource is { Amount: 0, ResourceType.IsNoSolid: false })
            Delete(resource);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken) => 
        await _db.SaveChangesAsync(cancellationToken);
}