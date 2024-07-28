using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IInvoiceExpenseRepository
{
    Task<InvoiceExpenseModel?> GetByIdAsync(Guid id);
    Task<InvoiceExpenseModel?> GetByExternalAsync(string external);
    Task<InvoiceExpenseModel?> GetByIdAndExternalAsync(Guid id, string external);
    Task<bool> CreateAsync(InvoiceExpenseModel record);
    bool Update(InvoiceExpenseModel record);
    bool UpdateRange(IEnumerable<InvoiceExpenseModel> records);
    Task<bool> ExistsByReferenceAsync(string reference);
}
