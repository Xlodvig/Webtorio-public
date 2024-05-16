using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models;
using Webtorio.Specifications.Resources;

namespace Webtorio.Application.Resources.Queries;

public class GetAllResource
{
    public record Query : IRequest<List<Resource>>;
    
    public class Handler : IRequestHandler<Query, List<Resource>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<List<Resource>> Handle(Query query, CancellationToken cancellationToken) => 
            await _repository.GetAllAsync(new StoredResourcesReadOnlySpec(), cancellationToken);
    }
}