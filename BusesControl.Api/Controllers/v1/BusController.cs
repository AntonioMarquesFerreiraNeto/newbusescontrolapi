using Asp.Versioning;
using BusesControl.Entities.Request;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v1/bus")]
public class BusController(
    IValidator<BusCreateRequest> _busCreateRequestValidator,
    IValidator<BusUpdateRequest> _busUpdateRequestValidator,
    IBusService _busService
) : ControllerBase
{
    [HttpGet("find")]
    public async Task<IActionResult> FindBySearch([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? search = null)
    {
        var response = await _busService.FindBySearchAsync(page, pageSize, search);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _busService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BusCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _busCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _busService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] BusUpdateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _busUpdateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _busService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpPatch("{id}/active")]
    public async Task<IActionResult> ToggleActive([FromRoute] Guid id)
    {
        await _busService.ToggleActiveAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/availability")]
    public async Task<IActionResult> ToggleAvailability([FromRoute] Guid id)
    {
        await _busService.ToggleAvailabilityAsync(id);
        return NoContent();
    }
}
