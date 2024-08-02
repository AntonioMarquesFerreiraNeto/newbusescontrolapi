using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IInvoiceExpenseRepository : IGenericRepository<InvoiceExpenseModel>
{
    Task<IEnumerable<InvoiceExpenseModel>> FindByFinancialAsync(Guid financialId);
    Task<InvoiceExpenseModel?> GetByIdAsync(Guid id);
    Task<InvoiceExpenseModel?> GetByExternalAsync(string external);
    Task<InvoiceExpenseModel?> GetByIdAndExternalAsync(Guid id, string external);
    Task<bool> ExistsByReferenceAsync(string reference);
}
