using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Responses;

namespace BusesControl.Services.v1.Interfaces;

public interface ISupportTicketService
{
    Task<IEnumerable<SupportTicketModel>> FindByStatusAsync(PaginationRequest request, SupportTicketStatusEnum? status = null);
    Task<SupportTicketResponse> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(SupportTicketCreateRequest request);
    Task<bool> CloseAsync(Guid id);
    Task<bool> CancelAsync(Guid id);
}
