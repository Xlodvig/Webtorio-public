using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models;
using Webtorio.Specifications.Deposits;

namespace Webtorio.Application.Deposits.Queries;

public class GetDeposit
{
    public record Query(int DepositId) : IRequest<ErrorOr<Deposit>>;
    
    public class Validator : AbstractValidator<Query>
    {
        public Validator() => 
            RuleFor(query => query.DepositId).GreaterThan(0);
    }
    
    public class Handler : IRequestHandler<Query, ErrorOr<Deposit>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<ErrorOr<Deposit>> Handle(Query query, CancellationToken cancellationToken) => 
            await _repository.GetAsync(new DepositByIdReadOnlySpec(query.DepositId), cancellationToken);
    }
}