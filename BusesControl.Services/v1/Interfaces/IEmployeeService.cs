﻿using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeModel>> FindBySearchAsync(int page, int pageSize, string? search = null);
    Task<EmployeeModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(EmployeeCreateRequest request);
    Task<bool> UpdateAsync(Guid id, EmployeeUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
    Task<SuccessResponse> ToggleTypeAsync(Guid id, EmployeeToggleTypeRequest request);
}
