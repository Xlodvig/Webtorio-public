using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Interfaces;

namespace Webtorio.Application.Resources.Commands;

public class AddResource
{
    public record Command(
        int ResourceTypeId, 
        double Amount
        ) : IRequest<ErrorOr<int>>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.ResourceTypeId).GreaterThan(0);
            RuleFor(command => command.Amount).GreaterThan(0);
        }
    }
    
    public class Handler : IRequestHandler<Command, ErrorOr<int>>
    {
        private readonly IRepository _repository;   

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<ErrorOr<int>> Handle(Command command, CancellationToken cancellationToken)
        {
            var result = await _repository
                .AddResourceAsync(command.ResourceTypeId, command.Amount, cancellationToken);

            if (!result.IsError) 
                await _repository.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}