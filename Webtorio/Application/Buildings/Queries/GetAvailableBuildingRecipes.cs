using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Errors;
using Webtorio.Models.Recipes;
using Webtorio.Models.StaticData;
using Webtorio.Specifications.Buildings;

namespace Webtorio.Application.Buildings.Queries;

public class GetAvailableBuildingRecipes
{
    public record Query(int BuildingId) : IRequest<ErrorOr<List<Recipe>>>;
    
    public class Validator : AbstractValidator<Query>
    {
        public Validator() => 
            RuleFor(query => query.BuildingId).GreaterThan(0);
    }
    
    public class Handler : IRequestHandler<Query, ErrorOr<List<Recipe>>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository) => 
            _repository = repository;


        public async Task<ErrorOr<List<Recipe>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var buildingResult =
                await _repository
                    .GetAsync(new BuildingByIdWithAvailableRecipesFullDataReadOnlySpec(query.BuildingId), cancellationToken);

            if (buildingResult.IsError)
                return buildingResult.Errors;

            if (buildingResult.Value.BuildingType is ManufactureBuildingType productiveBuildingType)
                return productiveBuildingType.BuildingRecipeMatches
                    .Select(brm => brm.Recipe)
                    .ToList();
            
            return Errors.BuildingType.NotManufacture;
        }
    }
}