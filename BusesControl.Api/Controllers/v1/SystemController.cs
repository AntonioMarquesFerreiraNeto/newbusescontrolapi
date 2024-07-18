using Asp.Versioning;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Roles = "System")]
[Route("api/v1/system")]
public class SystemController(
    ISystemService _systemService
) : ControllerBase
{
    /// <summary>
    /// Alteração de senha de usuário de sistema automatizado
    /// </summary>
    [HttpPatch("user/password")]
    public async Task<IActionResult> AutomatedChangePasswordUserSystem()
    {
        var response = await _systemService.AutomatedChangePasswordUserSystem();
        return Ok(response);
    }

    /// <summary>
    /// Pagamento de faturas automatizado
    /// </summary>
    [HttpPatch("invoice/automated-payment")]
    public async Task<IActionResult> AutomatedPayment([FromQuery] DateTime date)
    {
        var response = await _systemService.AutomatedPaymentAsync(date);
        return Ok(response);
    }

    /// <summary>
    /// Processa faturas que estão atrasadas
    /// </summary>
    [HttpPatch("invoice/overdue/processing")]
    public async Task<IActionResult> AutomatedOverdueInvoiceProcessing()
    {
        var response = await _systemService.AutomatedOverdueInvoiceProcessingAsync();
        return Ok(response);
    }

    /// <summary>
    /// Processa contratos que podem ser concluídos
    /// </summary>
    [HttpPatch("contract/finalization")]
    public async Task<IActionResult> AutomatedContractFinalization()
    {
        var response = await _systemService.AutomatedContractFinalizationAsync();
        return Ok(response);
    }

    /// <summary>
    /// Cancela processo de rescisão que imprimiram apenas o pdf da rescisão
    /// </summary>
    [HttpPatch("contract/cancel/process/termination")]
    public async Task<IActionResult> AutomatedCancelProcessTermination()
    {
        var response = await _systemService.AutomatedCancelProcessTerminationAsync();
        return Ok(response);
    }
}
