namespace BusesControl.Services.v1.Interfaces;

public interface IGenerationPdfService
{
    Task<byte[]> ContractForCustomerAsync(Guid contractId, Guid customerId);
}
