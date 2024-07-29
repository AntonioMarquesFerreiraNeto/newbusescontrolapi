using Asp.Versioning;
using BusesControl.Entities.Requests;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v1/contract/{contractId}/bus/replacement")]
public class ContractBusReplacementController(
    IValidator<ContractBusReplacementCreateRequest> _contractBusReplacementCreateRequestValidator,
    IContractBusReplacementService _contractBusReplacementService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FindByContract([FromRoute] Guid contractId)
    {
        var response = await _contractBusReplacementService.FindByContractAsync(contractId);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid contractId, [FromRoute] Guid id)
    {
        var response = await _contractBusReplacementService.GetByIdAsync(id, contractId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid contractId, [FromBody] ContractBusReplacementCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _contractBusReplacementCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _contractBusReplacementService.CreateAsync(contractId, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid contractId, [FromRoute] Guid id)
    {
        await _contractBusReplacementService.DeleteAsync(contractId, id);
        return NoContent();
    }
}
