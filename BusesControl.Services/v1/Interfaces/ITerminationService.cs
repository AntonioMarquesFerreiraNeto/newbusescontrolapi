using BusesControl.Entities.Requests;
using BusesControl.Entities.Response;

namespace BusesControl.Services.v1.Interfaces;

public interface ITerminationService
{
    Task<SuccessResponse> CreateAsync(Guid contractId, TerminationCreateRequest request);
}
