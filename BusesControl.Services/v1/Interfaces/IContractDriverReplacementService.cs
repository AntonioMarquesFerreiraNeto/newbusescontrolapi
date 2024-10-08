﻿using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IContractDriverReplacementService
{
    Task<PaginationResponse<ContractDriverReplacementModel>> FindByContractAsync(Guid contractId);
    Task<ContractDriverReplacementModel> GetByIdAsync(Guid id, Guid contractId);
    Task<bool> CreateAsync(Guid contractId, ContractDriverReplacementCreateRequest request);
    Task<bool> DeleteAsync(Guid contractId, Guid id);
}
