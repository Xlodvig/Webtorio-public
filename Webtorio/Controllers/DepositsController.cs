using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Webtorio.Application.Deposits.Commands;
using Webtorio.Application.Deposits.Queries;
using Webtorio.Contracts.Deposits;
using Webtorio.Contracts.ViewModels;

namespace Webtorio.Controllers;


[Route("deposits")]
public class DepositsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public DepositsController(IMediator mediator) => 
        _mediator = mediator;

    /// <summary>
    /// Получить все доступные депозиты
    /// </summary>
    /// <returns></returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(DepositShortViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDeposits()
    {
        var allDeposits = await _mediator.Send(new GetAllDeposits.Query());
    
        return Ok(allDeposits.Adapt<IEnumerable<DepositShortViewModel>>());
    }
    
    /// <summary>
    /// Получить депозит по id
    /// </summary>
    /// <param name="id"> id депозита </param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DepositViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDeposit(int id)
    {
        var depositResult = await _mediator.Send(new GetDeposit.Query(id));
        
        return depositResult.Match(
            onValue: deposit => Ok(deposit.Adapt<DepositViewModel>()),
            onError: errors => Problem(errors));
    }

    /// <summary>
    /// Добыть ресурсы из депозита по id
    /// </summary>
    /// <param name="id"> id депозита ресурсов </param>
    /// <param name="request"> Количество добываемого ресурса </param>
    /// <returns></returns>
    [HttpPatch("{id:int}/mine-resource")]
    public async Task<IActionResult> MineResource(int id, MineResourceRequest request)
    {
        var success = await _mediator.Send(new MineResource.Command(id, request.Amount));

        return success.Match(
            onValue: _ => NoContent(),
            onError: errors => Problem(errors));
    }
}