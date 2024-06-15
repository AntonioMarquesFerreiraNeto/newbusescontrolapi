using BusesControl.Entities.Models;
using BusesControl.Entities.Request;
using BusesControl.Entities.Response;

namespace BusesControl.Services.v1.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeModel>> FindBySearchAsync(int pageSize, int pageNumber, string? search = null);
    Task<EmployeeModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(EmployeeCreateRequest request);
    Task<bool> UpdateAsync(Guid id, EmployeeUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
    Task<SuccessResponse> ToggleTypeAsync(Guid id, EmployeeToggleTypeRequest request);
}
