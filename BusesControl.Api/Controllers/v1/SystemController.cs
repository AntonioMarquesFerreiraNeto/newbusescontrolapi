using Asp.Versioning;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin")]
[Route("api/v1/system")]
public class SystemController(
    ISystemService _systemService
) : ControllerBase
{
    /// <summary>
    /// Pagamento de faturas automatizado
    /// </summary>
    [HttpGet("automated-payment-invoice")]
    public async Task<IActionResult> AutomatedPayment([FromQuery] DateTime date)
    {
        var response = await _systemService.AutomatedPaymentAsync(date);
        return Ok(response);
    }

    /// <summary>
    /// Atualizar faturas que estão atrasadas
    /// </summary>
    [HttpGet("automated-overdue-invoice-processing")]
    public async Task<IActionResult> AutomatedOverdueInvoiceProcessing()
    {
        var response = await _systemService.AutomatedOverdueInvoiceProcessingAsync();
        return Ok(response);
    }
}
