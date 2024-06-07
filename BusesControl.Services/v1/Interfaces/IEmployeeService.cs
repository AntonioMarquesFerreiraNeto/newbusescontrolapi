using BusesControl.Entities.Request;

namespace BusesControl.Services.v1.Interfaces;

public interface IEmployeeService
{
    Task<bool> CreateAsync(EmployeeCreateRequest request);
}
