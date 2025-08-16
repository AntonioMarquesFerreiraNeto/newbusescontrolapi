using Asp.Versioning;
using BusesControl.Entities.Responses.v1;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v1/via-cep-integration/{cep}")]
public class ViaCepIntegrationController(
    IViaCepIntegrationService _viaCepIntegrationService
) : ControllerBase
{
    /// <summary>
    /// Retorna endereço pelo CEP
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetAddressByCep([FromRoute] string cep)
    {
        var response = await _viaCepIntegrationService.GetAddressByCepAsync(cep);
        return Ok(response);
    }
}
