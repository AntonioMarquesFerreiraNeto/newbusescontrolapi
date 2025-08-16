using Asp.Versioning;
using BusesControl.Entities.Responses.v1;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[Route("api/v1/legal/entity/details")]
[ApiVersion("1.0")]
[ApiController]
[Authorize]
public class LegalEntityDetailsController(
    ICustomerService _customerService
) : ControllerBase
{
    /// <summary>
    /// Retorna detalhes públicos de empresas pelo CNPJ
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(typeof(LegalEntityDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetDetails([FromQuery] string? cnpj)
    {
        var response = await _customerService.LegalPersonConsultationByCnpjAsync(cnpj);
        
        return Ok(response);
    }
}
