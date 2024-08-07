﻿using BusesControl.Entities.Models.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface ICustomerContractService
{
    Task<bool> CreateForContractAsync(IEnumerable<Guid> customersId, Guid contractId);
    Task<bool> UpdateForContractAsync(IEnumerable<Guid> customersId, Guid contractId);
    Task<bool> ToggleProcessTerminationWithOutValidationAsync(CustomerContractModel record);
    Task<bool> ToggleActiveForTerminationAsync(CustomerContractModel record);
}
