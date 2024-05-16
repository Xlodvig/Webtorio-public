using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Models;
using Webtorio.Specifications.Resources;

namespace Webtorio.Application.Resources.Queries;

public class GetResource
{
    public record Query(int ResourceTypeId) : IRequest<ErrorOr<Resource>>;
    
    public class Validator : AbstractValidator<Query>
    {
        public Validator() => 
            RuleFor(query => query.ResourceTypeId).GreaterThan(0);
    }
    
    public class Handler : IRequestHandler<Query, ErrorOr<Resource>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<ErrorOr<Resource>> Handle(Query query, CancellationToken cancellationToken)
        {
            var resourceResult = await _repository.GetAsync(
                new ResourceByResourceTypeIdReadOnlySpec(query.ResourceTypeId), cancellationToken);

            if (resourceResult.IsError)
                return resourceResult.Errors;

            return resourceResult.Value;
        }
    }
}