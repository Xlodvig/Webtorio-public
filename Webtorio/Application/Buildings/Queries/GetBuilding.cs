using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Specifications.Buildings;

namespace Webtorio.Application.Buildings.Queries;

public class GetBuilding
{
    public record Query(int BuildingId) : IRequest<ErrorOr<Building>>;
    
    public class Validator : AbstractValidator<Query>
    {
        public Validator() => 
            RuleFor(query => query.BuildingId).GreaterThan(0);
    }
    
    public class Handler : IRequestHandler<Query, ErrorOr<Building>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<ErrorOr<Building>> Handle(Query query, CancellationToken cancellationToken) => 
            await _repository.GetAsync(new BuildingByIdReadOnlySpec(query.BuildingId), cancellationToken);
    }
}