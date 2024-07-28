using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IInvoiceRepository
{
    Task<IEnumerable<InvoiceModel>> FindByDueDateForSystemAsync(DateOnly date, bool expenseOnly);
    Task<IEnumerable<InvoiceModel>> FindByStatusForSystemWithFinancialAsync(InvoiceStatusEnum status);
    Task<InvoiceModel?> GetByIdWithFinancialAsync(Guid id);
    Task<InvoiceModel?> GetByIdAndExternalAsync(Guid id, string externalId);
    Task<bool> CreateAsync(InvoiceModel record);
    bool Update(InvoiceModel record);
    Task<bool> ExistsByReferenceAsync(string reference);
}
