using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BusesControl.Business.v1;

public class UserBusiness(
    INotificationApi _notificationApi,
    UserManager<UserModel> _userManager
) : IUserBusiness
{
    public async Task<bool> ValidateForCreateAsync(string email, string role)
    {
        if (role != "Admin" && role != "Assistant")
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.User.InvalidRole
            );
            return false;
        }

        var userRecord = await _userManager.FindByEmailAsync(email);
        if (userRecord is not null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.User.Exists
            );
            return false;
        }

        return true;
    }
}
