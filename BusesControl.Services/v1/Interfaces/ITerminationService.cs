using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Response;

namespace BusesControl.Services.v1.Interfaces;

public interface ITerminationService
{
    Task<IEnumerable<TerminationModel>> FindByContractAsync(Guid contractId, string? search);
    Task<SuccessResponse> CreateAsync(Guid contractId, TerminationCreateRequest request);
}
