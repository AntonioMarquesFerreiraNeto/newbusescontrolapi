using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Responses;

namespace BusesControl.Services.v1.Interfaces;

public interface IFinancialService
{
    Task<IEnumerable<FinancialModel>> FindBySearchAsync(PaginationRequest request);
    Task<FinancialResponse> GetByIdAsync(Guid id);
    Task<bool> CreateRevenueAsync(FinancialRevenueCreateRequest request);
    Task<bool> InactiveRevenueAsync(Guid id);
    Task<bool> CreateExpenseAsync(FinancialExpenseCreateRequest request);
    Task<bool> UpdateDetailsAsync(Guid id, FinancialUpdateDetailsRequest request);
    Task<bool> CreateInternalAsync(ContractModel contractRecord);
    Task<bool> InactiveInternalAsync(Guid contractId, Guid customerId);
}
