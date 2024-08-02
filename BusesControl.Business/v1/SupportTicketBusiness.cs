using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class SupportTicketBusiness(
    INotificationContext _notificationContext,
    ISupportTicketRepository _supportTicketRepository
) : ISupportTicketBusiness
{
    public async Task<SupportTicketModel> GetForCancelOrCloseAsync(Guid id)
    {
        var record = await _supportTicketRepository.GetByIdOptionalEmployeeAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SupportTicket.NotFound
            );
            return default!;
        }

        if (record.Status != SupportTicketStatusEnum.Open && record.Status != SupportTicketStatusEnum.InProgress)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SupportTicket.NotOpenOrInProgress
            );
            return default!;
        }

        return record;
    }

    public async Task<SupportTicketModel> GetForCreateSupportTicketMessageAsync(Guid id, Guid? employeeId = null)
    {
        var record = await _supportTicketRepository.GetByIdOptionalEmployeeAsync(id, employeeId);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SupportTicket.NotFound
            );
            return default!;
        }

        if (record.Status != SupportTicketStatusEnum.Open && record.Status != SupportTicketStatusEnum.InProgress)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SupportTicket.NotOpenOrInProgress
            );
            return default!;
        }

        return record;
    }
}
