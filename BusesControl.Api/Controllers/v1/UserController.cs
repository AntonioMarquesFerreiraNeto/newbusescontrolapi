using BusesControl.Entities.Request;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/users")]
public class UserController(
    IValidator<UserToggleActiveRequest> _userToggleActiveRequestValidator,
    IValidator<UserSetNickNameRequest> _userSetNickNameRequestValidator,
    IValidator<UserResetPasswordStepCodeRequest> _userResetPasswordStepCodeRequestValidator,
    IValidator<UserResetPasswordStepResetTokenRequest> _userResetPasswordStepResetTokenRequestValidator,
    IValidator<UserResetPasswordStepNewPasswordRequest> _userResetPasswordStepNewPasswordRequestValidator,
    IUserService _userService
) : ControllerBase
{
    [HttpPatch("set-nickname/me")]
    [Authorize]
    public async Task<IActionResult> SetNickname([FromBody] UserSetNickNameRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userSetNickNameRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _userService.SetNicknameAsync(request);
        return Ok();
    }

    [HttpPatch("{id}/active")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleActive([FromRoute] Guid id, [FromBody] UserToggleActiveRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userToggleActiveRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _userService.ToggleActiveAsync(id, request);
        return Ok();
    }

    [HttpPatch("reset-password/step-code")]
    public async Task<IActionResult> ResetPasswordStepCode([FromBody] UserResetPasswordStepCodeRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userResetPasswordStepCodeRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _userService.ResetPasswordStepCodeAsync(request);
        return Ok(response);
    }

    [HttpPatch("reset-password/step-reset-token")]
    public async Task<IActionResult> ResetPasswordStepResetToken([FromBody] UserResetPasswordStepResetTokenRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userResetPasswordStepResetTokenRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _userService.ResetPasswordStepResetTokenAsync(request);
        return Ok(response);
    }

    [HttpPatch("reset-password/step-new-password")]
    public async Task<IActionResult> ResetPasswordStepNewPassword([FromBody] UserResetPasswordStepNewPasswordRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userResetPasswordStepNewPasswordRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _userService.ResetPasswordStepNewPasswordAsync(request);
        return Ok(response);
    }
}
