using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Interfaces;
using Webtorio.Specifications.Deposits;

namespace Webtorio.Application.Deposits.Commands;

public class MineResource
{
    public record Command(
        int DepositId,
        double Amount
    ) : IRequest<ErrorOr<Success>>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.DepositId).GreaterThan(0);
            RuleFor(command => command.Amount).GreaterThan(0);
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
            var depositResult = await _repository
                .GetAsync(new DepositByIdSpec(command.DepositId), cancellationToken);

            if (depositResult.IsError)
                return depositResult.Errors;
            
            var success = await depositResult.Value
                .MineAndStoreResourceAsync(command.Amount, _repository, _buildingWorkService, cancellationToken);

            if (!success.IsError)
                await _repository.SaveChangesAsync(cancellationToken);

            return success;
        }
    }
}