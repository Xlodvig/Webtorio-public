using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models;
using Webtorio.Specifications.Items;

namespace Webtorio.Application.Resources.Queries;

public class GetAllItems
{
    public record Query : IRequest<List<Item>>;
    
    public class Handler : IRequestHandler<Query, List<Item>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<List<Item>> Handle(Query query, CancellationToken cancellationToken) => 
            await _repository.GetAllAsync(new StoredItemsReadOnlySpec(), cancellationToken);
    }
}