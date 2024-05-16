using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Errors;
using Webtorio.Models.Buildings;
using Webtorio.Specifications.Buildings;
using Webtorio.Specifications.Slots;

namespace Webtorio.Application.Slots.Commands;

public class AddBuildingToSlot
{
    public record Command(
        int SlotId, 
        int BuildingId
        ) : IRequest<ErrorOr<Success>>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.SlotId).GreaterThan(0);
            RuleFor(command => command.BuildingId).GreaterThan(0);
        }
    }
    
    public class Handler : IRequestHandler<Command, ErrorOr<Success>>
    {
        private readonly IRepository _repository;
        private readonly BuildingWorkService _buildingWorkService;

        public Handler(IRepository repository, BuildingWorkService buildingWorkService)
        {
            _repository = repository;
            _buildingWorkService = buildingWorkService;
        }

        public async Task<ErrorOr<Success>> Handle(Command command, CancellationToken cancellationToken)
        {
            var slotResult = await _repository
                .GetAsync(new SlotByIdSpec<Slot>(command.SlotId), cancellationToken);

            if (slotResult.IsError)
                return slotResult.Errors;

            var slot = slotResult.Value;

            if (slot is { BuildingId: not null })
                return Errors.Slot.Occupied(slot.Id);

            var buildingResult = await _repository
                .GetAsync(new BuildingByIdSpec(command.BuildingId), cancellationToken);

            if (buildingResult.IsError)
                return buildingResult.Errors;

            var building = buildingResult.Value;

            if (building.State != BuildingState.Stored)
                return Errors.Building.NotStored;

            if ((building is ExtractiveBuilding && slot is not DepositSlot) || 
                (building is ManufactureBuilding or GeneratorBuilding && slot is DepositSlot))
                return Errors.Slot.InappropriateBuilding;

            var result = await _buildingWorkService
                .AddBuildingToSlotAsync(building, slot, cancellationToken);

            if (result.IsError)
                return result.Errors;

            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}