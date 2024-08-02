using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IExcelService
{
    Task<FileResponse> GenerateFinancialAsync();
    Task<FileResponse> GenerateContractAsync();
    Task<FileResponse> GenerateInvoiceByFinancialAsync(Guid financialId);
    Task<FileResponse> GenerateInvoiceExpenseByFinancialAsync(Guid financialId);
}
