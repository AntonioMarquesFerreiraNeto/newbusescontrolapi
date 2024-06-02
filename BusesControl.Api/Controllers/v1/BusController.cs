using BusesControl.Entities.Request;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/bus")]
public class BusController(
    IValidator<BusCreateRequest> _busCreateRequestValidator,
    IValidator<BusUpdateRequest> _busUpdateRequestValidator,
    IBusService _busService
) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var record = await _busService.GetByIdAsync(id);
        return Ok(record);
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
}
