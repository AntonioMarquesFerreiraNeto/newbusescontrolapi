using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IFinancialService
{
    Task<PaginationResponse<FinancialModel>> FindBySearchAsync(PaginationRequest request);
    Task<FinancialResponse> GetByIdAsync(Guid id);
    Task<bool> CreateRevenueAsync(FinancialRevenueCreateRequest request);
    Task<bool> InactiveRevenueAsync(Guid id);
    Task<bool> InactiveExpenseAsync(Guid id);
    Task<bool> CreateExpenseAsync(FinancialExpenseCreateRequest request);
    Task<bool> UpdateDetailsAsync(Guid id, FinancialUpdateDetailsRequest request);
    Task<bool> CreateInternalAsync(ContractModel contractRecord);
    Task<bool> InactiveInternalAsync(Guid contractId, Guid customerId);
}
