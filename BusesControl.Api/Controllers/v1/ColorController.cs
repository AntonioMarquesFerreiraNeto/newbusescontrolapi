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
[Route("api/v1/colors")]
public class ColorController(
    IValidator<ColorCreateRequest> _colorCreateRequestvalidator,
    IValidator<ColorUpdateRequest> _colorUpdateRequestvalidator,
    IColorService _colorService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FindBySearch([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? search)
    {
        var response = await _colorService.FindBySearchAsync(page, pageSize, search);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _colorService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ColorCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _colorCreateRequestvalidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _colorService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ColorUpdateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _colorUpdateRequestvalidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _colorService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpPatch("{id}/active")]
    public async Task<IActionResult> ToggleActive([FromRoute] Guid id)
    {
        await _colorService.ToggleActiveAsync(id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _colorService.DeleteAsync(id);
        return NoContent();
    }
}
