using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class NotificationService(
    IUserService _userService,
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    INotificationRepository _notificationRepository
) : INotificationService
{
    public async Task<IEnumerable<NotificationModel>> GetAllForAdminAsync(PaginationRequest request)
    {
        var records = await _notificationRepository.GetAllAsync(request.Page, request.PageSize);
        return records;
    }

    public async Task<IEnumerable<NotificationModel>> FindMyNotificationsAsync(PaginationRequest request)
    {
        var loggedUser = _userService.FindAuthenticatedUser();
        var records = await _notificationRepository.FindMyNotificationsAsync(loggedUser.Role, request.Page, request.PageSize);
        
        return records;
    }

    public async Task<NotificationModel> GetByIdAsync(Guid id)
    {
        var record = await _notificationRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Notification.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> SendInternalNotificationAsync(string title, string message, NotificationAccessLevelEnum accessLevel)
    {
        var notificationRequest = new NotificationCreateRequest
        {
            Title = title,
            Message = message,
            AccessLevel = accessLevel
        };
        await CreateAsync(notificationRequest, system: true);

        return true;
    }

    public async Task<bool> CreateAsync(NotificationCreateRequest request, bool system = false)
    {
        var senderId = system is false ? _userService.FindAuthenticatedUser().EmployeeId : null;

        var record = new NotificationModel
        {
            SenderId = senderId,
            Title = request.Title,
            Message = request.Message,
            SenderType = senderId is null ? NotificationSenderTypeEnum.System : NotificationSenderTypeEnum.Employee,
            AccessLevel = request.AccessLevel
        };
        await _notificationRepository.CreateAsync(record);  
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var record = await _notificationRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Notification.NotFound
            );
            return default!;
        }

        _notificationRepository.Delete(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
