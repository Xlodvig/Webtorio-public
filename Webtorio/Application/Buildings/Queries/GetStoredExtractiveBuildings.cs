using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Specifications.Buildings;

namespace Webtorio.Application.Buildings.Queries;

public class GetStoredExtractiveBuildings
{
    public record Query : IRequest<List<ExtractiveBuilding>>;
    
    public class Handler : IRequestHandler<Query, List<ExtractiveBuilding>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<List<ExtractiveBuilding>> Handle(Query query, CancellationToken cancellationToken) =>
            await _repository
                .GetAllFirstByBuildingTypeAsync(
                    new StoredBuildingsReadOnlySpec<ExtractiveBuilding>(), cancellationToken);
    }
}