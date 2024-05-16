using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Specifications.Buildings;

namespace Webtorio.Application.Buildings.Queries;

public class GetStoredManufactureBuildings
{
    public record Query : IRequest<List<ManufactureBuilding>>;
    
    public class Handler : IRequestHandler<Query, List<ManufactureBuilding>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;
        
        public async Task<List<ManufactureBuilding>> Handle(Query query, CancellationToken cancellationToken) =>
            await _repository
                .GetAllFirstByBuildingTypeAsync(
                    new StoredBuildingsReadOnlySpec<ManufactureBuilding>(), cancellationToken);
    }
}