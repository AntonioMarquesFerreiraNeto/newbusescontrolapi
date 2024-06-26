﻿using Asp.Versioning;
using BusesControl.Entities.Request;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/users-registration-queue")]
public class UserRegistrationQueueController(
    IValidator<UserRegistrationStepCodeRequest> _userRegistrationStepCodeRequestValidator,
    IValidator<UserRegistrationStepTokenRequest> _userRegistrationStepTokenRequestValidator,
    IValidator<UserRegistrationStepPasswordRequest> _userRegistrationStepPasswordRequestValidator,
    IUserRegistrationQueueService _userRegistrationQueueService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FindBySearch([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? search = null)
    {
        var response = await _userRegistrationQueueService.FindBySearchAsync(page, pageSize, search);
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
        var validation = ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userRegistrationStepCodeRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _userRegistrationQueueService.RegistrationUserStepCodeAsync(request);
        return Ok(response);
    }

    [HttpPatch("step-token")]
    public async Task<IActionResult> RegistrationUserStepToken([FromBody] UserRegistrationStepTokenRequest request)
    {
        var validation = ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userRegistrationStepTokenRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _userRegistrationQueueService.RegistrationUserStepTokenAsync(request);
        return Ok(response);
    }

    [HttpPatch("step-password")]
    public async Task<IActionResult> ResetPasswordStepNewPassword([FromBody] UserRegistrationStepPasswordRequest request)
    {
        var validation = ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userRegistrationStepPasswordRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

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
