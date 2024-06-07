using BusesControl.Entities.Request;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/employee")]
public class EmployeeController(
    IEmployeeService _employeeService
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateRequest request)
    {
        await _employeeService.CreateAsync(request);
        return NoContent();
    }
}
