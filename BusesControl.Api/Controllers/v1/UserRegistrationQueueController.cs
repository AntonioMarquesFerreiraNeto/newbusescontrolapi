using BusesControl.Entities.Request;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/users-registration-queue")]
public class UserRegistrationQueueController(
    IUserRegistrationQueueService _userRegistrationQueueService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? search = null)
    {
        var response = await _userRegistrationQueueService.FindAsync(pageSize, pageNumber, search);
        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateForEmployee([FromBody] UserRegistrationCreateRequest request)
    {
        var response = await _userRegistrationQueueService.CreateForEmployeeAsync(request);
        return Ok(response);
    }

    [HttpPatch("step-code")]
    public async Task<IActionResult> RegistrationUserStepCode([FromBody] UserRegistrationStepCodeRequest request)
    {
        var response = await _userRegistrationQueueService.RegistrationUserStepCodeAsync(request);
        return Ok(response);
    }

    [HttpPatch("step-token")]
    public async Task<IActionResult> RegistrationUserStepToken([FromBody] UserRegistrationStepTokenRequest request)
    {
        var response = await _userRegistrationQueueService.RegistrationUserStepTokenAsync(request);
        return Ok(response);
    }

    [HttpPatch("step-password")]
    public async Task<IActionResult> ResetPasswordStepNewPassword([FromBody] UserRegistrationStepPasswordRequest request)
    {
        var response = await _userRegistrationQueueService.RegistrationUserStepPasswordAsync(request);
        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var response = await _userRegistrationQueueService.DeleteAsync(id);
        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}/approved")]
    public async Task<IActionResult> Approved([FromRoute] Guid id)
    {
        var response = await _userRegistrationQueueService.AprrovedAsync(id);
        return Ok(response);
    }
}
