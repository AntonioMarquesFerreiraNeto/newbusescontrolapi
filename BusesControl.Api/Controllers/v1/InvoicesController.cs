using Asp.Versioning;
using BusesControl.Commons.Notification.Interfaces;
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
[Route("api/v1/invoices")]
public class InvoicesController(
    INotificationApi _notificationApi,
    IValidator<InvoicePaymentRequest> _invoicePaymentRequestValidator,
    IExcelService _excelService,
    IInvoiceService _invoiceService
) : ControllerBase
{
    /// <summary>
    /// Busca relatório em excel das faturas pelo financeiro
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("report/{financialId}/excel")]
    public async Task<IActionResult> GetExcelByFinancial([FromRoute] Guid financialId)
    {
        var fileResponse = await _excelService.GenerateInvoiceByFinancialAsync(financialId);
        if (_notificationApi.HasNotification)
        {
            return StatusCode(_notificationApi.StatusCodes!.Value, _notificationApi.Details);
        }

        return File(fileResponse.FileContent, fileResponse.ContentType, fileResponse.FileName);
    }

    /// <summary>
    /// Realiza pagamento da fatura pelo ID
    /// </summary>
    /// <response code="200">Retorna sucesso da requisição</response>
    /// <response code="400">Retorna erro de requisição inválida</response>
    /// <response code="401">Retorna erro de não autorizado</response>
    /// <response code="404">Retorna erro de não não encontrado</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("{id}/payment")]
    public async Task<IActionResult> Payment([FromRoute] Guid id, [FromBody] InvoicePaymentRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _invoicePaymentRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _invoiceService.PaymentAsync(id, request);
        return Ok(response);
    }
}
