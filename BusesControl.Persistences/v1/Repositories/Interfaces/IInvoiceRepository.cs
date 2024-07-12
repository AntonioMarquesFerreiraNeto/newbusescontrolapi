using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IInvoiceRepository
{
    Task<InvoiceModel?> GetByIdWithFinancialAsync(Guid id);
    Task<bool> CreateAsync(InvoiceModel record);
    bool Update(InvoiceModel record);
    Task<bool> ExistsByReferenceAsync(string reference);
}
