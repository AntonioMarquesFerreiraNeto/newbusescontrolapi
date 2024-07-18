using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Response;
using BusesControl.Entities.Responses;

namespace BusesControl.Services.v1.Interfaces;

public interface IContractService
{
    Task<ContractResponse> GetByIdAsync(Guid id);
    Task<PdfCoResponse> GetGeneratedContractForCustomerAsync(Guid id, Guid customerId);
    Task<PdfCoResponse> StartProcessTerminationAsync(Guid id, Guid customerId);
    Task<IEnumerable<ContractModel>> FindByOptionalStatusAsync(int page, int pageSize, ContractStatusEnum? status);
    Task<bool> CreateAsync(ContractCreateRequest request);
    Task<bool> UpdateAsync(Guid id, ContractUpdateRequest request);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> DeniedAsync(Guid id);
    Task<bool> WaitingReviewAsync(Guid id);
    Task<SuccessResponse> ApproveAsync(Guid id);
    Task<SuccessResponse> StartProgressAsync(Guid id);
    Task<bool> CompletedWithoutValidationAsync(ContractModel record);
}
