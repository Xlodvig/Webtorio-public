using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models;
using Webtorio.Specifications.Deposits;

namespace Webtorio.Application.Deposits.Queries;

public class GetAllDeposits
{
    public record Query : IRequest<List<Deposit>>;
    
    public class Handler : IRequestHandler<Query, List<Deposit>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<List<Deposit>> Handle(Query query, CancellationToken cancellationToken) => 
            await _repository.GetAllAsync(new DepositsReadOnlySpec(), cancellationToken);
    }
}