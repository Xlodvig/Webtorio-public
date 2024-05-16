using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Errors;
using Webtorio.Models.Buildings;
using Webtorio.Specifications.Buildings;

namespace Webtorio.Application.Buildings.Commands;

public class SwitchBuildingOnOff
{
    public record Command(int BuildingId) : IRequest<ErrorOr<Success>>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator() => 
            RuleFor(command => command.BuildingId).GreaterThan(0);
    }
    
    public class Handler : IRequestHandler<Command, ErrorOr<Success>>
    {
        private readonly IRepository _repository;
        private readonly BuildingWorkService _buildingWorkService;

        public Handler(BuildingWorkService buildingWorkService, IRepository repository)
        {
            _buildingWorkService = buildingWorkService;
            _repository = repository;
        }

        public async Task<ErrorOr<Success>> Handle(Command command, CancellationToken cancellationToken)
        {
            var buildingResult = await _repository
                .GetAsync(new BuildingByIdSpec(command.BuildingId), cancellationToken);

            if (buildingResult.IsError)
                return buildingResult.Errors;

            var building = buildingResult.Value;
            
            if (building is { State: BuildingState.Stored })
                return Errors.BuildingWork.ErrorChangeStoredState;
            
            var result = await _buildingWorkService.TurnOnOffAsync(building, cancellationToken);
            
            if (result.IsError)
                return result.Errors;

            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}