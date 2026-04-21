using Asp.Versioning;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/two/fa")]
    [AllowAnonymous]
    [EnableRateLimiting("two-fa-policy")]
    public class TwoFaController(
        ITwoFaService _twoFaService,
        IValidator<TwoFaCheckForNewRequest> _twoFaCheckForNewRequestValidator,
        IValidator<CreateTwoRequest> _createTwoFaRequestValidator,
        IValidator<TwoFaValidateCodeRequest> _twoFaValidateCodeRequestValidator
    ) : ControllerBase
    {
        /// <summary>
        /// Verifica a necessidade do processo de autenticação em dois fatores por e-mail
        /// </summary>
        [HttpPost("check/for/new")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckForNew([FromBody] TwoFaCheckForNewRequest request)
        {
            var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _twoFaCheckForNewRequestValidator);
            if (validation != null)
                return BadRequest(validation);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _twoFaService.CheckForNew(ip, request);

            return NoContent();
        }

        /// <summary>
        /// Cria nova entidade de autenticação em dois fatores
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [EnableRateLimiting("auth-policy")]
        public async Task<IActionResult> Create([FromBody] CreateTwoRequest request)
        {
            var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _createTwoFaRequestValidator);
            if (validation != null)
                return BadRequest(validation);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _twoFaService.CreateAsync(ip, request);

            return NoContent();
        }

        /// <summary>
        /// Validar código da autenticação em dois fatores
        /// </summary>
        [HttpPost("check/code")]
        [ProducesResponseType(typeof(TwoFaValidateCodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [EnableRateLimiting("auth-policy")]
        public async Task<IActionResult> CheckCode([FromBody] TwoFaValidateCodeRequest request)
        {
            var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _twoFaValidateCodeRequestValidator);
            if (validation != null)
                return BadRequest(validation);

            var response = await _twoFaService.ValidateCodeAsync(request);

            return Ok(response);
        }
    }
}
