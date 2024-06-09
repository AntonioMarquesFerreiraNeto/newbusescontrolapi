using BusesControl.Entities.Request;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
//[Authorize(Roles = "Admin")]
[Route("api/v1/employee")]
public class EmployeeController(
    IValidator<EmployeeCreateRequest> _employeeCreateRequestValidator,
    IEmployeeService _employeeService
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _employeeCreateRequestValidator);
        if (validation is not null)
        {
            return BadRequest(validation);
        }

        await _employeeService.CreateAsync(request);
        return NoContent();
    }
}
