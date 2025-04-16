using Asp.Versioning;
using BusesControl.Entities.Requests.v1;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
    /// <summary>
    /// Retorna solicitações de registro de usuário
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="403">Retorna erro de acesso negado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("auth-policy")]
    [HttpGet]
    public async Task<IActionResult> FindBySearch([FromQuery] PaginationRequest request)
    {
        var response = await _userRegistrationQueueService.FindBySearchAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Cria uma nova solicitação de registro de usuário para funcionário
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("auth-policy")]
    [HttpPost]
    public async Task<IActionResult> CreateForEmployee([FromBody] UserRegistrationCreateRequest request)
    {
        var response = await _userRegistrationQueueService.CreateForEmployeeAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Registro de usuário step code
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("auth-policy")]
    [HttpPatch("step-code")]
    public async Task<IActionResult> RegistrationUserStepCode([FromBody] UserRegistrationStepCodeRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userRegistrationStepCodeRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _userRegistrationQueueService.RegistrationUserStepCodeAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Registro de usuário step token
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("auth-policy")]
    [HttpPatch("step-token")]
    public async Task<IActionResult> RegistrationUserStepToken([FromBody] UserRegistrationStepTokenRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userRegistrationStepTokenRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _userRegistrationQueueService.RegistrationUserStepTokenAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Registro de usuário step nova senha
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPatch("step-password")]
    public async Task<IActionResult> ResetPasswordStepNewPassword([FromBody] UserRegistrationStepPasswordRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _userRegistrationStepPasswordRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _userRegistrationQueueService.RegistrationUserStepPasswordAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Remove uma solicitação de registro de usuário pelo ID
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var response = await _userRegistrationQueueService.DeleteAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Aprova uma solicitação de registro de usuário pelo ID
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}/approve")]
    public async Task<IActionResult> Approve([FromRoute] Guid id)
    {
        var response = await _userRegistrationQueueService.AprroveAsync(id);
        return Ok(response);
    }
}
