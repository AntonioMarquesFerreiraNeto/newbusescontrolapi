using BusesControl.Entities.Models;

namespace BusesControl.Services.v1.Interfaces;

public interface IFinancialService
{
    Task<bool> CreateForContractAsync(ContractModel contractRecord);
    Task<bool> ToggleActiveForTerminationAsync(Guid contractId, Guid customerId);
}
