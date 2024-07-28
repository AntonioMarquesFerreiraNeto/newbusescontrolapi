using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class SupportTicketBusiness(
    INotificationApi _notificationApi,
    ISupportTicketRepository _supportTicketRepository
) : ISupportTicketBusiness
{
    public async Task<SupportTicketModel> GetForCancelOrCloseAsync(Guid id)
    {
        var record = await _supportTicketRepository.GetByIdOptionalEmployeeAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SupportTicket.NotFound
            );
            return default!;
        }

        if (record.Status != SupportTicketStatusEnum.Open && record.Status != SupportTicketStatusEnum.InProgress)
        {
            _notificationApi.SetNotification(
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
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SupportTicket.NotFound
            );
            return default!;
        }

        if (record.Status != SupportTicketStatusEnum.Open && record.Status != SupportTicketStatusEnum.InProgress)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SupportTicket.NotOpenOrInProgress
            );
            return default!;
        }

        return record;
    }
}
