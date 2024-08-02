using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierModel>> FindBySearchAsync(PaginationRequest request);
    Task<SupplierModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(SupplierCreateRequest request);
    Task<bool> UpdateAsync(Guid id, SupplierUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
}
