using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IInvoiceRepository : IGenericRepository<InvoiceModel>
{
    Task<IEnumerable<InvoiceModel>> FindByFinancialAsync(Guid financialId);
    Task<IEnumerable<InvoiceModel>> FindByDueDateForSystemAsync(DateOnly date);
    Task<IEnumerable<InvoiceModel>> FindByStatusForSystemWithFinancialAsync(InvoiceStatusEnum status);
    Task<InvoiceModel?> GetByIdWithFinancialAsync(Guid id);
    Task<InvoiceModel?> GetByIdAndExternalAsync(Guid id, string externalId);
    Task<bool> ExistsByReferenceAsync(string reference);
}
