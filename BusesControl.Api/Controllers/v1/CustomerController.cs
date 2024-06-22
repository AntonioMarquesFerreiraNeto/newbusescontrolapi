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
[Route("api/v1/customer")]
public class CustomerController(
    IValidator<CustomerCreateRequest> _customerCreateRequestValidator,
    IValidator<CustomerUpdateRequest> _customerUpdateRequestValidator,
    ICustomerService _customerService
) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _customerService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> FindBySearch([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? search)
    {
        var result = await _customerService.FindBySearchAsync(page, pageSize, search);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _customerCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _customerService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CustomerUpdateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _customerUpdateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _customerService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpPatch("{id}/active")]
    public async Task<IActionResult> ToggleActive([FromRoute] Guid id)
    {
        await _customerService.ToggleActiveAsync(id);
        return NoContent();
    }
}
