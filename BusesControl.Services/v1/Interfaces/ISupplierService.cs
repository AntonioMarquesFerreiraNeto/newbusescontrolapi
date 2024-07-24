using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;

namespace BusesControl.Services.v1.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierModel>> FindBySearchAsync(PaginationRequest request);
    Task<SupplierModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(SupplierCreateRequest request);
    Task<bool> UpdateAsync(Guid id, SupplierUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
}
