using BusesControl.Entities.Requests;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/financials")]
public class FinancialController(
    IValidator<FinancialRevenueCreateRequest> _financialRevenueCreateRequestValidator,
    IValidator<FinancialUpdateDetailsRequest> _financialUpdateDetailsRequestValidator,
    IValidator<FinancialExpenseCreateRequest> _financialExpenseCreateRequestValidator,
    IFinancialService _financialService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FindBySearch([FromQuery] PaginationRequest request)
    {
        var response = await _financialService.FindBySearchAsync(request);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _financialService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpPost("revenue")]
    public async Task<IActionResult> CreateRevenue([FromBody] FinancialRevenueCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _financialRevenueCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _financialService.CreateRevenueAsync(request);
        return NoContent();
    }

    [HttpPost("expense")]
    public async Task<IActionResult> CreateExpense([FromBody] FinancialExpenseCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _financialExpenseCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _financialService.CreateExpenseAsync(request);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}/revenue/inactive")]
    public async Task<IActionResult> InactiveRevenue([FromRoute] Guid id)
    {
        await _financialService.InactiveRevenueAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/details")]
    public async Task<IActionResult> UpdateDetails([FromRoute] Guid id, [FromBody] FinancialUpdateDetailsRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _financialUpdateDetailsRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _financialService.UpdateDetailsAsync(id, request);
        return NoContent();
    }
}
