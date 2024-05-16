using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Specifications.Slots;

namespace Webtorio.Application.Slots.Queries;

public class GetSlot
{
    public record Query(int SlotId) : IRequest<ErrorOr<Slot>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator() => 
            RuleFor(query => query.SlotId).GreaterThan(0);
    }
    
    public class Handler : IRequestHandler<Query, ErrorOr<Slot>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<ErrorOr<Slot>> Handle(Query query, CancellationToken cancellationToken) => 
            await _repository.GetAsync(new SlotByIdReadOnlySpec<Slot>(query.SlotId), cancellationToken);
    }
}