using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Errors;
using Webtorio.Models.Buildings;
using Webtorio.Specifications.Slots;

namespace Webtorio.Application.Slots.Commands;

public class RemoveBuildingFromSlot
{
    public record Command(int SlotId) : IRequest<ErrorOr<Success>>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator() => 
            RuleFor(command => command.SlotId).GreaterThan(0);
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
            var slotId = command.SlotId;
            
            var slotResult = await _repository.GetAsync(new SlotByIdSpec<Slot>(slotId), cancellationToken);

            if (slotResult.IsError)
                return slotResult.Errors;

            var slot = slotResult.Value;

            if (slot.Building is null)
                return Errors.Slot.BuildingNotFound(slotId);

            var result = await _buildingWorkService.StoreBuildingFromSlotAsync(slot, cancellationToken);

            if (result.IsError)
                return result.Errors;

            await _repository.SaveChangesAsync(cancellationToken);
        
            return Result.Success;
        }
    }
}