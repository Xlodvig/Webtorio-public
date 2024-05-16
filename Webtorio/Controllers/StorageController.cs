using MediatR;
using Microsoft.AspNetCore.Mvc;
using Webtorio.Application.Resources.Commands;
using Webtorio.Application.Resources.Queries;
using Webtorio.Contracts.Storage;
using Webtorio.Models;

namespace Webtorio.Controllers;

[Route("storage")]
public class StorageController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public StorageController(IMediator mediator) => 
        _mediator = mediator;

    /// <summary>
    /// Получить все ресурсы и здания, находящиеся на складе
    /// </summary>
    /// <returns></returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(List<Item>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllItems()
    {
        var allItems = await _mediator.Send(new GetAllItems.Query());
        
        return Ok(allItems);
    }

    /// <summary>
    /// Получить все ресурсы
    /// </summary>
    /// <returns></returns>
    [HttpGet("resources")]
    [ProducesResponseType(typeof(List<Resource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllResource()
    {
        var allResource = await _mediator.Send(new GetAllResource.Query());

        return Ok(allResource);
    }

    /// <summary>
    /// Получить ресурс по его типу
    /// </summary>
    /// <param name="resourceTypeId"> id типа ресурса </param>
    /// <returns></returns>
    [HttpGet("resource/{resourceTypeId:int}")]
    [ProducesResponseType(typeof(Resource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResource(int resourceTypeId)
    {
        var result = await _mediator.Send(new GetResource.Query(resourceTypeId));

        return result.Match(
            onValue: resource => Ok(resource),
            onError: errors => Problem(errors));
    }

    /// <summary>
    /// Добавить ресурс на склад
    /// </summary>
    /// <remarks>
    /// Возвращается id добавленного ресурса 
    /// </remarks>
    /// <param name="request"> id типа ресурса, и его количество </param>
    /// <returns></returns>
    [HttpPost("add-resource")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddResource(AddResourceRequest request)
    {
        var result = await _mediator.Send(
            new AddResource.Command(request.ResourceTypeId, request.Amount));

        return result.Match(
            onValue: id => Ok(id),
            onError: errors => Problem(errors));
    }

    /// <summary>
    /// Удалить ресурс со склада
    /// </summary>
    /// <param name="request"> id типа ресурса, и его количество </param>
    /// <returns></returns>
    [HttpPost("remove-resource")]
    public async Task<IActionResult> RemoveResource(RemoveResourceRequest request)
    {
        var success = await _mediator.Send(
            new RemoveResource.Command(request.ResourceTypeId, request.Amount));

        return success.Match(
            onValue: _ => NoContent(),
            onError: errors => Problem(errors));
    }
    
    /// <summary>
    /// Получить доступные виды топлива
    /// </summary>
    /// <returns></returns>
    [HttpGet("available-fuel")]
    [ProducesResponseType(typeof(List<Resource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableFuel()
    {
        var allFuel = await _mediator.Send(new GetAvailableFuel.Query());

        return Ok(allFuel);
    }
}