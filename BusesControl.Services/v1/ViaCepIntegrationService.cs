using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace BusesControl.Services.v1;

public class ViaCepIntegrationService(
    AppSettings _appSettings,
    INotificationContext _notificationContext
) : IViaCepIntegrationService
{
    public async Task<AddressResponse> GetAddressByCepAsync(string cep)
    {
        if (String.IsNullOrEmpty(cep) || cep.Length < 9)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Address.NotFoundOrInvalid
            );
            return default!;
        }

        var httpClient = new HttpClient();
        
        var result = await httpClient.GetAsync($"{_appSettings.ViaCep.Url}/{cep}/json");
        if (!result.IsSuccessStatusCode)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Address.Invalid
            );
            return default!;
        }

        var response = await result.Content.ReadFromJsonAsync<AddressResponse>();
        if (response is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Address.NotFound
            );
            return default!;
        }

        return response;
    }
}
