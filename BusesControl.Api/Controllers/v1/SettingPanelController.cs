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
public class SettingPanelController(
    IValidator<SettingPanelCreateRequest> _settingPanelCreateRequestValidator,
    IValidator<SettingPanelUpdateRequest> _settingPanelUpdateRequestValidator,
    ISettingPanelService _settingPanelService
) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _settingPanelService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] int page, [FromQuery] int pageSize)
    {
        var response = await _settingPanelService.FindAsync(page, pageSize);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SettingPanelCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _settingPanelCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _settingPanelService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SettingPanelUpdateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _settingPanelUpdateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _settingPanelService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _settingPanelService.DeleteAsync(id);
        return NoContent();
    }
}
