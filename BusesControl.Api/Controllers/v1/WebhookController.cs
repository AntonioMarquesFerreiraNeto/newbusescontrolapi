using Asp.Versioning;
using BusesControl.Entities.Requests;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/webhook")]
public class WebhookController(
    IWebhookService _webhookService
) : ControllerBase
{
    [HttpPost("payment-pix")]
    public async Task<IActionResult> PaymentPix([FromBody] PaymentPixRequest request)
    {
        await _webhookService.PaymentPixAsync(Request.Headers["asaas-access-token"], request);
        return NoContent();
    }
}
