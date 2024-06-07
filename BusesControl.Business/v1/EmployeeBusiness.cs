using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class EmployeeBusiness(
    INotificationApi _notificationApi,
    IEmployeeRepository _employeeRepository
) : IEmployeeBusiness
{
    public async Task<bool> ExistsByEmailOrPhoneNumberOrCpfAsync(string email, string phoneNumber, string cpf, Guid? id = null)
    {
        var exists = await _employeeRepository.ExistsByEmailOrPhoneNumberOrCpfAsync(email, phoneNumber, cpf, id);
        if (exists) 
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.Employee.Exists
            );
            return false;
        }

        return true;
    }
}
