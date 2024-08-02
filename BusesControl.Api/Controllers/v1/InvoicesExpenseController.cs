using Asp.Versioning;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Requests.v1;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v1/invoices/expense")]
public class InvoicesExpenseController(
    IValidator<InvoiceExpensePaymentRequest> _invoiceExpensePaymentRequestValidator,
    INotificationContext _notificationContext,
    IExcelService _excelService,
    IInvoiceExpenseService _invoiceExpenseService
) : ControllerBase
{
    /// <summary>
    /// Retorna relatório em excel das faturas de despesa pelo financeiro
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
        var fileResponse = await _excelService.GenerateInvoiceExpenseByFinancialAsync(financialId);
        if (_notificationContext.HasNotification)
        {
            return StatusCode(_notificationContext.StatusCodes!.Value, _notificationContext.Details);
        }

        return File(fileResponse.FileContent, fileResponse.ContentType, fileResponse.FileName);
    }

    /// <summary>
    /// Realiza pagamento da fatura de despesa pelo ID
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
    public async Task<IActionResult> Payment([FromRoute] Guid id, [FromBody] InvoiceExpensePaymentRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _invoiceExpensePaymentRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        var response = await _invoiceExpenseService.PaymentAsync(id, request);
        return Ok(response);
    }
}
