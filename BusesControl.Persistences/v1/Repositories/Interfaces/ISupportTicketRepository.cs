using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface ISupportTicketRepository
{
    Task<SupportTicketModel?> GetByIdOptionalEmployeeAsync(Guid id, Guid? employeeId = null);
    Task<SupportTicketModel?> GetByIdOptionalEmployeeWithIncludesAsync(Guid id, Guid? employeeId = null);
    Task<IEnumerable<SupportTicketModel>> FindByStatusAsync(Guid? employeeId = null, SupportTicketStatusEnum ? status = null, int page = 0, int pageSize = 0);
    Task<bool> CreateAsync(SupportTicketModel record);
    bool Update(SupportTicketModel record);
    Task<bool> ExistsByReferenceAsync(string reference);
}
