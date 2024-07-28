using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Response;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace BusesControl.Business.v1;

public class InvoiceExpenseBusiness(
    AppSettings _appSettings,
    INotificationApi _notificationApi,
    IInvoiceExpenseRepository _invoiceExpenseRepository
) : IInvoiceExpenseBusiness
{
    public async Task<InvoiceExpenseModel> GetForPaymentAsync(Guid id)
    {
        var record = await _invoiceExpenseRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.InvoiceExpense.NotFound
            );
            return default!;
        }

        if (record.Status != InvoiceExpenseStatusEnum.Pending)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.InvoiceExpense.NotPending
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> ValidateBalanceInAssasAsync(decimal pricePayment)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var httpResult = await httpClient.GetAsync($"{_appSettings.Assas.Url}/finance/balance");
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.Commons.BalanceAssasFailure
            );
            return false;
        }

        var response = await httpResult.Content.ReadFromJsonAsync<AssasBalanceDTO>();
        if (response is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.Commons.BalanceAssasFailure
            );
            return false;
        }

        if (response.Balance < pricePayment)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.InvoiceExpense.InsufficientBalance
            );
            return false;
        }

        return true;
    }

    public bool ValidateLoggedUserForJustCountPayment(UserAuthResponse loggedUser)
    {
        if (loggedUser.Role != "Admin" && loggedUser.Role != "Assistant")
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.JustCountInternalUsersOnly
            );
            return false;
        }

        return true;
    }
}
