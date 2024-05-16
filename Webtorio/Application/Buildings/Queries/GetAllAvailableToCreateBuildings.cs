using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models.StaticData;
using Webtorio.Specifications.Buildings;

namespace Webtorio.Application.Buildings.Queries;

public class GetAllAvailableToCreateBuildings
{
    public record Query : IRequest<List<BuildingType>>;
    
    public class Handler : IRequestHandler<Query, List<BuildingType>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<List<BuildingType>> Handle(Query query, CancellationToken cancellationToken) => 
            await _repository.GetAllAsync(new BuildingTypesAvailableToCreateReadOnlySpec(), cancellationToken);
    }
}