using Asp.Versioning;
using BusesControl.Entities.Requests.v1;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v1/question")]
    [ApiController]
    [Authorize]
    public class QuestionController(
        IValidator<GenerativePostRequest> _generativePostRequestValidator,
        IGenerativeService _generativeService
    ) : ControllerBase
    {
        /// <summary>
        /// Realiza uma pergunta simples para nossa IA
        /// </summary>
        /// <response code="200">Retorna sucesso da requisição</response>
        /// <response code="400">Retorna erro de requisição inválida</response>
        /// <response code="401">Retorna erro de não autorizado</response>
        /// <response code="500">Retorna erro interno do servidor</response>
        [HttpPost("post")]
        public async Task<IActionResult> Post(GenerativePostRequest request)
        {
            var validate = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _generativePostRequestValidator);
            if (validate != null)
            {
                return BadRequest(validate);
            }

            var response = await _generativeService.Post(request);

            return Ok(response);
        }
    }
}
