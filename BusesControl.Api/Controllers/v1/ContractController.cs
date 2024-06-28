using Asp.Versioning;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Requests;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiVersion("1.0")]
[ApiController]
[Authorize]
[Route("api/v1/contracts")]
public class ContractController(
    IValidator<ContractCreateRequest> _contractCreateRequestValidator,
    IValidator<ContractUpdateRequest> _contractUpdateRequestValidator,
    IContractService _contractService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FindByOptionalStatus([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] ContractStatusEnum? status)
    {
        var response = await _contractService.FindByOptionalStatusAsync(page, pageSize, status);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _contractService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContractCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _contractCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _contractService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ContractUpdateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _contractUpdateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _contractService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpPatch("{id}/waiting-review")]
    public async Task<IActionResult> WaitingReview([FromRoute] Guid id)
    {
        await _contractService.WaitingReviewAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/denied")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Denied([FromRoute] Guid id)
    {
        await _contractService.DeniedAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve([FromRoute] Guid id)
    {
        var response =  await _contractService.ApproveAsync(id);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _contractService.DeleteAsync(id);
        return NoContent();
    }
}
