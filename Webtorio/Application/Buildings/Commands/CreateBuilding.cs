using ErrorOr;
using FluentValidation;
using MediatR;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Errors;
using Webtorio.Models.Buildings;
using Webtorio.Models.Constants;
using Webtorio.Models.StaticData;
using Webtorio.Specifications.Buildings;

namespace Webtorio.Application.Buildings.Commands;

public class CreateBuilding
{
    public record Command(int BuildingTypeId) : IRequest<ErrorOr<Building>>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator() => 
            RuleFor(command => command.BuildingTypeId).GreaterThan(0);
    }
    
    public class Handler : IRequestHandler<Command, ErrorOr<Building>>
    {
        private readonly IRepository _repository;
        
        public Handler(IRepository repository) => 
            _repository = repository;

        public async Task<ErrorOr<Building>> Handle(Command command, CancellationToken cancellationToken)
        {
            var buildingTypeResult = await _repository
                .GetAsync(new BuildingTypeByIdSpec(command.BuildingTypeId), cancellationToken);

            if (buildingTypeResult.IsError)
                return buildingTypeResult.Errors;
        
            var buildingType = buildingTypeResult.Value;

            Building? building = buildingType switch
            {
                ExtractiveBuildingType type => new ExtractiveBuilding(type.Name, buildingType),
                ManufactureBuildingType type => new ManufactureBuilding(type.Name, buildingType),
                GeneratorBuildingType type => new GeneratorBuilding(type.Name, buildingType),
                _ => null,
            };

            if (building is null) 
                return Errors.Building.NotImplemented(buildingType.Name);
            
            if (buildingType.Energy == Energy.Burner)
                building.SelectedFuelResourceTypeId = ItemTypeId.Coal;

            await _repository.AddAsync(building, cancellationToken);

            await _repository.SaveChangesAsync(cancellationToken);

            return building;
        }
    }
}