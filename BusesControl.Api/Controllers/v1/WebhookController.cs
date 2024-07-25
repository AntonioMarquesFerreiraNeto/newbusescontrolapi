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
[Authorize(Roles = "System, Admin")]
[Route("api/v1/webhook")]
public class WebhookController(
    IValidator<WebhookCreateRequest> _webhookCreateValidator,
    IWebhookService _webhookService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _webhookService.GetAllAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _webhookService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WebhookCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _webhookCreateValidator);
        if (validation is not null)
        {
            return BadRequest(validation);  
        }

        await _webhookService.CreateAsync(request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var response = await _webhookService.DeleteAsync(id);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("payment-pix")]
    public async Task<IActionResult> PaymentPix([FromBody] PaymentPixRequest request)
    {
        await _webhookService.PaymentPixAsync(Request.Headers["asaas-access-token"], request);
        return NoContent();
    }
}
