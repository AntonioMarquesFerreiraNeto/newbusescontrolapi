using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface ITerminationService
{
    Task<PaginationResponse<TerminationModel>> FindByContractAsync(Guid contractId, string? search);
    Task<SuccessResponse> CreateAsync(Guid contractId, TerminationCreateRequest request);
}
