using Asp.Versioning;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/via-cep-integration/{cep}")]
public class ViaCepIntegrationController(
    IViaCepIntegrationService _viaCepIntegrationService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAddressByCep([FromRoute] string cep)
    {
        var response = await _viaCepIntegrationService.GetAddressByCepAsync(cep);
        return Ok(response);
    }
}
