using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Webtorio.Application.Slots.Commands;
using Webtorio.Application.Slots.Queries;
using Webtorio.Contracts.ViewModels.Slots;

namespace Webtorio.Controllers;


[Route("slots")]
public class SlotsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public SlotsController(IMediator mediator) => 
        _mediator = mediator;

    /// <summary>
    /// Получить слот по id
    /// </summary>
    /// <param name="id"> id слота </param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SlotViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSlot(int id)
    {
        var slotResult = await _mediator.Send(new GetSlot.Query(id));
        
        return slotResult.Match(
            onValue: slot => Ok(slot.Adapt<SlotViewModel>()),
            onError: errors => Problem(errors));
    }
    
    /// <summary>
    /// Разместить здание в слоте
    /// </summary>
    /// <param name="slotId"> id слота </param>
    /// <param name="buildingId"> id здания </param>
    /// <returns></returns>
    [HttpPatch("{slotId:int}/add-building/{buildingId:int}")]
    public async Task<IActionResult> AddBuildingToSlot(int slotId, int buildingId)
    {
        var success = await _mediator.Send(new AddBuildingToSlot.Command(slotId, buildingId));

        return success.Match(
            onValue: _ => NoContent(),
            onError: errors => Problem(errors));
    }

    /// <summary>
    /// Убрать здание из слота на склад
    /// </summary>
    /// <param name="slotId"> id слота </param>
    /// <returns></returns>
    [HttpPatch("{slotId:int}/remove-building")]
    public async Task<IActionResult> RemoveBuildingFromSlot(int slotId)
    {
        var success = await _mediator.Send(new RemoveBuildingFromSlot.Command(slotId));

        return success.Match(
            onValue: _ => NoContent(),
            onError: errors => Problem(errors));
    }

    /// <summary>
    /// Получить все не депозитные слоты
    /// </summary>
    /// <remarks>
    /// Не депозитные слоты: для размещения производственных и генерирующих зданий
    /// </remarks>
    /// <returns></returns>
    [HttpGet("all-non-deposit")]
    [ProducesResponseType(typeof(NonDepositSlotViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllNonDepositSlots()
    {
        var allNonDepositSlots = await _mediator.Send(new GetAllNonDepositSlots.Query());

        return Ok(allNonDepositSlots.Adapt<IEnumerable<NonDepositSlotViewModel>>());
    }
}