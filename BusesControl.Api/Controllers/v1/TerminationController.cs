using Asp.Versioning;
using BusesControl.Entities.Requests;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/{contractId}/terminations")]
public class TerminationController(
    ITerminationService _terminationService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FindByContract([FromRoute] Guid contractId, [FromRoute] string? search)
    {
        var response = await _terminationService.FindByContractAsync(contractId, search);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid contractId, [FromBody] TerminationCreateRequest request)
    {
        var response = await _terminationService.CreateAsync(contractId, request);
        return Ok(response);
    }
}
