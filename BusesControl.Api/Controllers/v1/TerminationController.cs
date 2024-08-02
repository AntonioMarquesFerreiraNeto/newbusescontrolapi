using Asp.Versioning;
using BusesControl.Entities.Requests;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/{contractId}/terminations")]
public class TerminationController(
    ITerminationService _terminationService
) : ControllerBase
{
    /// <summary>
    /// Retorna rescisões por contrato
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> FindByContract([FromRoute] Guid contractId, [FromRoute] string? search)
    {
        var response = await _terminationService.FindByContractAsync(contractId, search);
        return Ok(response);
    }

    /// <summary>
    /// Cria uma nova rescisão de contrato
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="403">Retorna erro de acesso negado</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid contractId, [FromBody] TerminationCreateRequest request)
    {
        var response = await _terminationService.CreateAsync(contractId, request);
        return Ok(response);
    }
}
