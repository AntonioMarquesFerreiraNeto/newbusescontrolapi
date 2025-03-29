using Asp.Versioning;
using BusesControl.Entities.Requests.v1;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v1/support/ticket/{ticketId}/messages")]
public class SupportTicketMessageController(
    IValidator<SupportTicketMessageCreateRequest> _supportTicketMessageCreateRequestValidator,
    ISupportTicketMessageService _supportTicketMessageService
) : ControllerBase
{
    /// <summary>
    /// Retorna mensagens de um ticket
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> FindByTicket([FromRoute] Guid ticketId)
    {
        var response = await _supportTicketMessageService.FindByTicketAsync(ticketId);

        return Ok(response);
    }

    /// <summary>
    /// Cria uma nova mensagem para um ticket de suporte pelo ticket
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
    public async Task<IActionResult> Create([FromRoute] Guid ticketId, [FromBody] SupportTicketMessageCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _supportTicketMessageCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _supportTicketMessageService.CreateAsync(ticketId, request);

        return Ok(response);
    }
}
