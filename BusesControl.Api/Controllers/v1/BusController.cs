using Asp.Versioning;
using BusesControl.Entities.Request;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v1/bus")]
public class BusController(
    IValidator<BusCreateRequest> _busCreateRequestValidator,
    IValidator<BusUpdateRequest> _busUpdateRequestValidator,
    IBusService _busService
) : ControllerBase
{
    /// <summary>
    /// Retorna ônibus
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("find")]
    public async Task<IActionResult> FindBySearch([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? search = null)
    {
        var response = await _busService.FindBySearchAsync(page, pageSize, search);
        return Ok(response);
    }

    /// <summary>
    /// Retorna ônibus pelo ID
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _busService.GetByIdAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Cria um novo ônibus
    /// </summary>
    /// <response code="204">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não não encontrado</response>
    /// <response code="409">Retorna erro de conflito</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BusCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _busCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _busService.CreateAsync(request);
        return NoContent();
    }

    /// <summary>
    /// Atualiza ônibus pelo ID
    /// </summary>
    /// <response code="204">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não não encontrado</response>
    /// <response code="409">Retorna erro de conflito</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] BusUpdateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _busUpdateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _busService.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>
    /// Ativa ou inativa ônibus pelo ID
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
    [HttpPatch("{id}/active")]
    public async Task<IActionResult> ToggleActive([FromRoute] Guid id)
    {
        await _busService.ToggleActiveAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Altera disponibilidade do ônibus pelo ID
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
    [HttpPatch("{id}/availability")]
    public async Task<IActionResult> ToggleAvailability([FromRoute] Guid id)
    {
        await _busService.ToggleAvailabilityAsync(id);
        return NoContent();
    }
}
