using Asp.Versioning;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v1/exports")]
[Authorize(Roles = "Admin")]
[ApiController]
public class ExportsController(
    IExportService _exportService,
    INotificationContext _notificationContext,
    IValidator<ExportCreateRequest> _exportCreateRequestValidator
) : ControllerBase
{
    /// <summary>
    /// Obter exportações paginado
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de autorização</response>
    /// <response code="403">Retorna erro de acesso negado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(typeof(PaginationResponse<ExportModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetPaginated([FromQuery] int page, [FromQuery] int pageSize)
    {
        var paginatedResult = await _exportService.GetPaginatedAsync(new PaginationRequest 
        {
            Page = page,
            PageSize = pageSize
        });

        return Ok(paginatedResult);
    }

    /// <summary>
    /// Obter arquivo de exportação
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de autorização</response>
    /// <response code="403">Retorna erro de acesso negado</response>
    /// <response code="404">Retorna erro de não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(typeof(PaginationResponse<ExportModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("file")]
    public async Task<IActionResult> GetExportFile([FromQuery] string name)
    {
        var fileResponse = await _exportService.GetFileAsync(name);
        if (_notificationContext.HasNotification)
        {
            return StatusCode(_notificationContext.StatusCodes!.Value, _notificationContext.Details);
        }

        return File(fileResponse.FileContent, fileResponse.ContentType, fileResponse.FileName);
    }

    /// <summary>
    /// Agendar exportação
    /// </summary>
    /// <response code="204">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="401">Retorna erro de autorização</response>
    /// <response code="403">Retorna erro de acesso negado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExportCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _exportCreateRequestValidator);
        if (validation != null)
        {
            return BadRequest(validation);
        }

        await _exportService.CreateAsync(request);

        return NoContent();
    }
}
