using BusesControl.Entities.Request;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/login")]
public class LoginController(
    IValidator<LoginRequest> _loginRequestValidator,
    IUserService _userService
) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _loginRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _userService.LoginAsync(request);
        return Ok(response);
    }
}
