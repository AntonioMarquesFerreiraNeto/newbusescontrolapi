using BusesControl.Entities.Response;

namespace BusesControl.Services.v1.Interfaces;

public interface IContractService
{
    Task<SuccessResponse> ApproveAsync(Guid id);
}
