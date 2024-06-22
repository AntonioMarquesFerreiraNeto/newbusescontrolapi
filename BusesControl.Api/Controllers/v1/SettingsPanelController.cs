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
[Route("api/v1/settings-panel")]
public class SettingsPanelController(
    IValidator<SettingsPanelCreateRequest> _settingsPanelCreateRequestValidator,
    IValidator<SettingsPanelUpdateRequest> _settingsPanelUpdateRequestValidator,
    ISettingsPanelService _settingsPanelService
) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _settingsPanelService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] int page, [FromQuery] int pageSize)
    {
        var response = await _settingsPanelService.FindAsync(page, pageSize);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SettingsPanelCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _settingsPanelCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _settingsPanelService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SettingsPanelUpdateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _settingsPanelUpdateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _settingsPanelService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _settingsPanelService.DeleteAsync(id);
        return NoContent();
    }
}
