using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IInvoiceRepository
{
    Task<IEnumerable<InvoiceModel>> FindByDueDateForSystemAsync(DateOnly date, bool expenseOnly);
    Task<IEnumerable<InvoiceModel>> FindByStatusForSystemAsync(InvoiceStatusEnum status);
    Task<InvoiceModel?> GetByIdWithFinancialAsync(Guid id);
    Task<bool> CreateAsync(InvoiceModel record);
    bool Update(InvoiceModel record);
    Task<bool> ExistsByReferenceAsync(string reference);
}
