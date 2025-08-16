using Asp.Versioning;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Services.v1;
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
    /// <summary>
    /// Retorna painel de configuração pelo ID
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(typeof(SettingPanelModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _settingPanelService.GetByIdAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Retorna paineis de configurações
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(typeof(PaginationResponse<SettingPanelModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] PaginationRequest request)
    {
        var response = await _settingPanelService.FindAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Retorna paineis pelo destinatário
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="403">Retorna erro de acesso negado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(typeof(IEnumerable<SettingPanelModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("parent/{parent}")]
    public async Task<IActionResult> FindByParent([FromRoute] SettingPanelParentEnum parent)
    {
        var response = await _settingPanelService.FindByParentAsync(parent);
        return Ok(response);
    }

    /// <summary>
    /// Cria painel de configuração
    /// </summary>
    /// <response code="204">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Atualiza painel de configuração pelo ID
    /// </summary>
    /// <response code="204">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Remove painel de configuração pelo ID
    /// </summary>
    /// <response code="204">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _settingPanelService.DeleteAsync(id);
        return NoContent();
    }
}
