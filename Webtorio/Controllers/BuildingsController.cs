using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Webtorio.Application.Buildings.Commands;
using Webtorio.Application.Buildings.Queries;
using Webtorio.Contracts.Buildings;
using Webtorio.Contracts.ViewModels;
using Webtorio.Contracts.ViewModels.Buildings;

namespace Webtorio.Controllers;

[Route("buildings")]
public class BuildingsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public BuildingsController(IMediator mediator) => 
        _mediator = mediator;

    /// <summary>
    /// Получить здание по id
    /// </summary>
    /// <param name="id"> id здания </param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BuildingViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBuilding(int id)
    {
        var buildingResult = await _mediator.Send(new GetBuilding.Query(id));

        return buildingResult.Match(
            onValue: building => Ok(building.Adapt<BuildingViewModel>()),
            onError: errors => Problem(errors));
    }
    
    /// <summary>
    /// Получить все возможные для создания здания, по типам
    /// </summary>
    /// <returns></returns>
    [HttpGet("available-to-create")]
    [ProducesResponseType(typeof(BuildingTypeViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAvailableToCreateBuildings()
    {
        var buildingTypes = await _mediator.Send(new GetAllAvailableToCreateBuildings.Query());

        return Ok(buildingTypes.Adapt<IEnumerable<BuildingTypeViewModel>>());
    }

    /// <summary>
    /// Создать здание
    /// </summary>
    /// <param name="request"> id типа здания </param>
    /// <remarks>
    /// Возможные значения:
    /// <code>
    /// BurnerMiningDrill = 100;
    /// ElectricMiningDrill = 101;
    /// StoneFurnace = 102;
    /// SteelFurnace = 103;
    /// ElectricFurnace = 104;
    /// OffshorePump = 105;
    /// Boiler = 106;
    /// SteamEngine = 107;
    /// </code>
    /// </remarks>
    [HttpPost("create")]
    [ProducesResponseType(typeof(BuildingViewModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBuilding(CreateBuildingRequest request)
    {
        var buildingResult = await _mediator.Send(new CreateBuilding.Command(request.BuildingTypeId));

        return buildingResult.Match(
            onValue: building => Ok(building.Adapt<BuildingViewModel>()),
            onError: errors => Problem(errors));
    }

    /// <summary>
    /// Получить добывающие здания размещённые на складе
    /// </summary>
    /// <remarks>
    /// Группировка по типам зданий, отображается по 1 зданию с наименьшим id 
    /// </remarks>
    /// <returns></returns>
    [HttpGet("stored/extractive")]
    [ProducesResponseType(typeof(ExtractiveBuildingViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStoredExtractiveBuildings()
    {
        var extractiveBuildings = await _mediator
            .Send(new GetStoredExtractiveBuildings.Query());

        return Ok(extractiveBuildings.Adapt<IEnumerable<ExtractiveBuildingViewModel>>());
    }

    /// <summary>
    /// Получить производственные здания размещённые на складе
    /// </summary>
    /// <remarks>
    /// Группировка по типам зданий, отображается по 1 зданию с наименьшим id 
    /// </remarks>
    /// <returns></returns>
    [HttpGet("stored/manufacture")]
    [ProducesResponseType(typeof(ManufactureBuildingViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStoredManufactureBuildings()
    {
        var manufactureBuildings = await _mediator
            .Send(new GetStoredManufactureBuildings.Query());

        return Ok(manufactureBuildings.Adapt<IEnumerable<ManufactureBuildingViewModel>>());
    }

    /// <summary>
    /// Получить генерирующие здания размещённые на складе
    /// </summary>
    /// <remarks>
    /// Группировка по типам зданий, отображается по 1 зданию с наименьшим id 
    /// </remarks>
    /// <returns></returns>
    [HttpGet("stored/generator")]
    [ProducesResponseType(typeof(GeneratorBuildingViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStoredGeneratorBuildings()
    {
        var generatorBuildings = await _mediator
            .Send(new GetStoredGeneratorBuildings.Query());

        return Ok(generatorBuildings.Adapt<IEnumerable<GeneratorBuildingViewModel>>());
    }
    
    /// <summary>
    /// ВКЛ/ВЫКЛ здания
    /// </summary>
    /// <param name="id"> id здания </param>
    /// <returns></returns>
    [HttpPatch("{id:int}/switch-on-off")]
    public async Task<IActionResult> SwitchBuildingOnOff(int id)
    {
        var switchResult = await _mediator.Send(new SwitchBuildingOnOff.Command(id));

        return switchResult.Match(
            onValue: _ => Ok(),
            onError: errors => Problem(errors));
    }

    /// <summary>
    /// Поменять тип топлива в здании
    /// </summary>
    /// <param name="buildingId"> id здания </param>
    /// <param name="resourceTypeId"> id типа ресурса </param>
    /// <returns></returns>
    [HttpPatch("{buildingId:int}/change-fuel/{resourceTypeId:int}")]    
    public async Task<IActionResult> ChangeFuel(int buildingId, int resourceTypeId)
    {
        var result = await _mediator.Send(new ChangeFuel.Command(buildingId, resourceTypeId));

        return result.Match(
            onValue: _ => Ok(),
            onError: errors => Problem(errors));
    }

    /// <summary>
    /// Получить все доступные для данного здания рецепты
    /// </summary>
    /// <param name="id"> id здания </param>
    /// <returns></returns>
    [HttpGet("{id:int}/available-recipes")]
    [ProducesResponseType(typeof(RecipeViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableRecipes(int id)
    {
        var result = await _mediator.Send(new GetAvailableBuildingRecipes.Query(id));

        return result.Match(
            onValue: recipes => Ok(recipes.Adapt<IEnumerable<RecipeViewModel>>()),
            onError: errors => Problem(errors));
    }
    
    /// <summary>
    /// Выбрать для данного здания рецепт по id
    /// </summary>
    /// <param name="buildingId"> id здания </param>
    /// <param name="recipeId"> id рецепта </param>
    /// <returns></returns>
    [HttpPatch("{buildingId:int}/select-recipe/{recipeId:int}")]
    public async Task<IActionResult> SelectRecipe(int buildingId, int recipeId)
    {
        var result = await _mediator.Send(new SelectRecipe.Command(buildingId, recipeId));
        
        return result.Match(
            onValue: _ => Ok(),
            onError: errors => Problem(errors));
    }
}