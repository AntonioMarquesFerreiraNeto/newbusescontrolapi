using Asp.Versioning;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Requests;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v1/support/tickets")]
public class SupportTicketController(
    IValidator<SupportTicketCreateRequest> _supportTicketCreateRequestValidator,
    ISupportTicketService _supportTicketService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FindByStatus([FromQuery] PaginationRequest request, [FromQuery] SupportTicketStatusEnum? status)
    {
        var response = await _supportTicketService.FindByStatusAsync(request, status);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _supportTicketService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Assistant")]
    public async Task<IActionResult> Create([FromBody] SupportTicketCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _supportTicketCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _supportTicketService.CreateAsync(request);
        return NoContent();
    }

    [HttpPatch("{id}/close")]
    [Authorize(Roles = "SupportAgent")]
    public async Task<IActionResult> Close([FromRoute] Guid id)
    {
        await _supportTicketService.CloseAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/cancel")]
    [Authorize(Roles = "SupportAgent")]
    public async Task<IActionResult> Cancel([FromRoute] Guid id)
    {
        await _supportTicketService.CancelAsync(id);
        return NoContent();
    }
}
