using Asp.Versioning;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v1/contract/{contractId}/bus/replacement")]
public class ContractBusReplacementController(
    IValidator<ContractBusReplacementCreateRequest> _contractBusReplacementCreateRequestValidator,
    IContractBusReplacementService _contractBusReplacementService
) : ControllerBase
{
    /// <summary>
    /// Retorna substituições de ônibus pelo contrato
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(typeof(PaginationResponse<ContractBusReplacementModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> FindByContract([FromRoute] Guid contractId)
    {
        var response = await _contractBusReplacementService.FindByContractAsync(contractId);
        return Ok(response);
    }

    /// <summary>
    /// Busca substituição de ônibus pelo contrato e ID
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(typeof(ContractBusReplacementModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid contractId, [FromRoute] Guid id)
    {
        var response = await _contractBusReplacementService.GetByIdAsync(id, contractId);
        return Ok(response);
    }

    /// <summary>
    /// Cria uma nova substituição de ônibus para o contrato
    /// </summary>
    /// <response code="204">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid contractId, [FromBody] ContractBusReplacementCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _contractBusReplacementCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _contractBusReplacementService.CreateAsync(contractId, request);
        return NoContent();
    }

    /// <summary>
    /// Remove substituição de ônibus pelo contrato e ID
    /// </summary>
    /// <response code="204">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid contractId, [FromRoute] Guid id)
    {
        await _contractBusReplacementService.DeleteAsync(contractId, id);
        return NoContent();
    }
}
