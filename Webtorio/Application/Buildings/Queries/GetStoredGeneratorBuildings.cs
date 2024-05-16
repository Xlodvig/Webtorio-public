using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Specifications.Buildings;

namespace Webtorio.Application.Buildings.Queries;

public class GetStoredGeneratorBuildings
{
    public record Query : IRequest<List<GeneratorBuilding>>;
    
    public class Handler : IRequestHandler<Query, List<GeneratorBuilding>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;
        
        public async Task<List<GeneratorBuilding>> Handle(Query query, CancellationToken cancellationToken) =>
            await _repository
                .GetAllFirstByBuildingTypeAsync(
                    new StoredBuildingsReadOnlySpec<GeneratorBuilding>(), cancellationToken);
    }
}