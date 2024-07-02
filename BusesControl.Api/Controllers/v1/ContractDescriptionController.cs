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
[Route("api/v1/contract-descriptions")]
public class ContractDescriptionController(
    IValidator<ContractDescriptionCreateRequest> _contractDescriptionCreateRequestValidator,
    IValidator<ContractDescriptionUpdateRequest> _contractDescriptionUpdateRequestValidator,
    IContractDescriptionService _contractDescriptionService
) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _contractDescriptionService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] int page, [FromQuery] int pageSize)
    {
        var response = await _contractDescriptionService.FindAsync(page, pageSize);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContractDescriptionCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _contractDescriptionCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _contractDescriptionService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ContractDescriptionUpdateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _contractDescriptionUpdateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _contractDescriptionService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _contractDescriptionService.DeleteAsync(id);
        return NoContent();
    }
}
