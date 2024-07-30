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
[Route("api/v1/contract/{contractId}/driver/replacement")]
public class ContractDriverReplacementController(
    IValidator<ContractDriverReplacementCreateRequest> _contractDriverReplacementCreateRequestValidator,
    IContractDriverReplacementService _contractDriverReplacementService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FindByContract([FromRoute] Guid contractId)
    {
        var response = await _contractDriverReplacementService.FindByContractAsync(contractId);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid contractId, [FromRoute] Guid id)
    {
        var response = await _contractDriverReplacementService.GetByIdAsync(id, contractId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid contractId, [FromBody] ContractDriverReplacementCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _contractDriverReplacementCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _contractDriverReplacementService.CreateAsync(contractId, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid contractId, [FromRoute] Guid id)
    {
        await _contractDriverReplacementService.DeleteAsync(contractId, id);
        return NoContent();
    }
}
