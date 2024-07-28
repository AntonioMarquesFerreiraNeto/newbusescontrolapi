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
[Route("api/v1/invoices/expense")]
public class InvoicesExpenseController(
    IValidator<InvoiceExpensePaymentRequest> _invoiceExpensePaymentRequestValidator,
    IInvoiceExpenseService _invoiceExpenseService
) : ControllerBase
{
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
