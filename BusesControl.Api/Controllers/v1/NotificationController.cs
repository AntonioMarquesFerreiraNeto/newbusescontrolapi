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
[Route("api/v1/notifications")]
public class NotificationController(
    IValidator<NotificationCreateRequest> _notificationCreateRequestValidator,
    INotificationService _notificationService
) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllForAdmin([FromQuery] PaginationRequest request)
    {
        var response = await _notificationService.GetAllForAdminAsync(request);
        return Ok(response);
    }

    [HttpGet("my")]
    public async Task<IActionResult> FindMyNotifications([FromQuery] PaginationRequest request)
    {
        var response = await _notificationService.FindMyNotificationsAsync(request);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _notificationService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] NotificationCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _notificationCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _notificationService.CreateAsync(request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _notificationService.DeleteAsync(id);
        return NoContent();
    }
}
