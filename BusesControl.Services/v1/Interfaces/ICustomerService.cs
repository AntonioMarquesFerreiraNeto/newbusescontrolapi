﻿using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerModel>> FindBySearchAsync(int page, int pageSize, string? search = null);
    Task<CustomerModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(CustomerCreateRequest request);
    Task<bool> UpdateAsync(Guid id, CustomerUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
}
