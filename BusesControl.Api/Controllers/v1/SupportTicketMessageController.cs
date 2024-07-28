using Asp.Versioning;
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
[Route("api/v1/support/ticket/{ticketId}/messages")]
public class SupportTicketMessageController(
    IValidator<SupportTicketMessageCreateRequest> _supportTicketMessageCreateRequestValidator,
    ISupportTicketMessageService _supportTicketMessageService
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid ticketId, [FromBody] SupportTicketMessageCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _supportTicketMessageCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _supportTicketMessageService.CreateAsync(ticketId, request);
        return NoContent();
    }
}
