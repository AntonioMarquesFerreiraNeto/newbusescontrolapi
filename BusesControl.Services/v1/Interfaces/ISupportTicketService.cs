using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface ISupportTicketService
{
    Task<PaginationResponse<SupportTicketModel>> FindByStatusAsync(PaginationRequest request, SupportTicketStatusEnum? status = null);
    Task<SupportTicketResponse> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(SupportTicketCreateRequest request);
    Task<bool> CloseAsync(Guid id);
    Task<bool> CancelAsync(Guid id);
}
