using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Specifications.Buildings;
using Webtorio.Specifications.ResourceTypes;

namespace Webtorio.Application.Buildings.Commands;

public class ChangeFuel
{
    public record Command(
        int BuildingId, 
        int ResourceTypeId
        ) : IRequest<ErrorOr<Success>>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.BuildingId).GreaterThan(0);
            RuleFor(command => command.ResourceTypeId).GreaterThan(0);
        }
    }
    
    public class Handler : IRequestHandler<Command, ErrorOr<Success>>
    {
        private readonly IRepository _repository;
            
        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<ErrorOr<Success>> Handle(Command command, CancellationToken cancellationToken)
        {
            var buildingResult = await _repository
                .GetAsync(new BuildingByIdSpec(command.BuildingId), cancellationToken);
            
            if (buildingResult.IsError)
                return buildingResult.Errors;

            var resourceTypeResult =
                await _repository.GetAsync(new ResourceTypeByIdSpec(command.ResourceTypeId), cancellationToken);

            if (resourceTypeResult.IsError)
                return resourceTypeResult.Errors;

            buildingResult.Value.SelectedFuelResourceTypeId = command.ResourceTypeId;

            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}