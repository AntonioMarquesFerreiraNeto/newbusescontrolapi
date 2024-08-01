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
