using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Interfaces;

namespace Webtorio.Application.Resources.Commands;

public class RemoveResource
{
    public record Command(
        int ResourceTypeId,
        double Amount
        ) : IRequest<ErrorOr<Success>>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.ResourceTypeId).GreaterThan(0);
            RuleFor(command => command.Amount).GreaterThan(0);
        }
    }
    
    public class Handler : IRequestHandler<Command, ErrorOr<Success>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<ErrorOr<Success>> Handle(Command command, CancellationToken cancellationToken)
        {
            var success = await _repository
                .RemoveResourceAsync(command.ResourceTypeId, command.Amount, cancellationToken);

            if (!success.IsError) 
                await _repository.SaveChangesAsync(cancellationToken);

            return success;
        }
    }
}