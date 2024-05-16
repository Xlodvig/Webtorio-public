using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Errors;
using Webtorio.Models.StaticData;
using Webtorio.Specifications.Buildings;

namespace Webtorio.Application.Buildings.Commands;

public class SelectRecipe
{
    public record Command(
        int BuildingId, 
        int RecipeId
        ) : IRequest<ErrorOr<Success>>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.BuildingId).GreaterThan(0);
            RuleFor(command => command.RecipeId).GreaterThan(0);
        }
    }
    
    public class Handler : IRequestHandler<Command, ErrorOr<Success>>
    {
        private readonly IRepository _repository;
        
        public Handler(IRepository repository) =>
            _repository = repository;

        public async Task<ErrorOr<Success>> Handle(Command command, CancellationToken cancellationToken)
        {
            var buildingResult =
                await _repository.GetAsync(new BuildingByIdWithAvailableRecipesSpec(command.BuildingId), cancellationToken);

            if (buildingResult.IsError)
                return buildingResult.Errors;

            if (buildingResult.Value.BuildingType is not ManufactureBuildingType manufactureBuildingType)
                return Errors.BuildingType.NotManufacture;
            
            var buildingRecipeMatch = manufactureBuildingType.BuildingRecipeMatches
                .SingleOrDefault(brm => brm.RecipeId == command.RecipeId);

            if (buildingRecipeMatch is null)
                return Errors.Recipe.NotAvailable;

            buildingResult.Value.SelectedRecipeId = buildingRecipeMatch.RecipeId;

            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}