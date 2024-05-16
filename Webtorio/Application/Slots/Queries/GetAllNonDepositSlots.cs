using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Specifications.Slots;

namespace Webtorio.Application.Slots.Queries;

public class GetAllNonDepositSlots
{
    public record Query : IRequest<List<Slot>>;

    public class Handler : IRequestHandler<Query, List<Slot>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<List<Slot>> Handle(Query query, CancellationToken cancellationToken) => 
            await _repository.GetAllAsync(new NonDepositSlotsReadOnlySpec(), cancellationToken);
    }
}