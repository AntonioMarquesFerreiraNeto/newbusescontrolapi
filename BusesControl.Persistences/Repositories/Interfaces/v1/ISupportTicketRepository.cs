using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface ISupportTicketRepository : IGenericRepository<SupportTicketModel>
{
    Task<SupportTicketModel?> GetByIdOptionalEmployeeAsync(Guid id, Guid? employeeId = null);
    Task<SupportTicketModel?> GetByIdOptionalEmployeeWithIncludesAsync(Guid id, Guid? employeeId = null);
    Task<IEnumerable<SupportTicketModel>> FindByStatusAsync(Guid? employeeId = null, SupportTicketStatusEnum? status = null, int page = 0, int pageSize = 0);
    Task<bool> ExistsByReferenceAsync(string reference);
}
