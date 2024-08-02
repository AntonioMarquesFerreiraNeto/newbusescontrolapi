using BusesControl.Entities.Models.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface ISupportTicketBusiness
{
    Task<SupportTicketModel> GetForCancelOrCloseAsync(Guid id);
    Task<SupportTicketModel> GetForCreateSupportTicketMessageAsync(Guid id, Guid? employeeId = null);
}
