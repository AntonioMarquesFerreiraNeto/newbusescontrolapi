using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace BusesControl.Services.v1;

public class InvoiceService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IInvoiceRepository _invoiceRepository
) : IInvoiceService
{
    private async Task<string> GenerateReferenceUniqueAsync()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var reference = "#";
        var random = new Random();
        var existsReference = true;

        while (existsReference)
        {
            for (int c = 0; c < 7; c++)
            {
                reference += chars[random.Next(chars.Length)];
            }
            existsReference = await _invoiceRepository.ExistsByReferenceAsync(reference);
        }

        return reference;
    }

    private async Task<string> CreateInAssasAsync(Guid id, string externalId, string descriptionInvoice, CreateInvoiceDTO createInvoice)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("access_token", AppSettingsAssas.Key);

        var createInvoiceInAssas = new 
        {
            billingType = "CREDIT_CARD",
            customer = externalId,
            dueDate = createInvoice.DueDate,
            value = createInvoice.Price,
            description = descriptionInvoice,
            externalReference = id
        };

        var httpResult = await httpClient.PostAsJsonAsync($"{AppSettingsAssas.Url}/payments", createInvoiceInAssas);
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.Unexpected
            );
            return default!;
        }

        var invoiceExternal = await httpResult.Content.ReadFromJsonAsync<CreateInvoiceInAssasDTO>();
        if (invoiceExternal is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.Unexpected
            );
            return default!;
        }

        return invoiceExternal.Id;
    }

    public async Task<bool> CreateForFinancialAsync(CreateInvoiceDTO createInvoice)
    {
        var title = createInvoice.PaymentType == PaymentTypeEnum.Single ? "Fatura única" : $"{createInvoice.Index}º fatura";
        var initialDescription = createInvoice.PaymentType == PaymentTypeEnum.Single ? "Fatura única" : $"{createInvoice.Index}º fatura";

        var record = new InvoiceModel
        {
            Reference = await GenerateReferenceUniqueAsync(),
            FinancialId = createInvoice.FinancialId,
            Title = title,
            Description = $"{initialDescription} referente ao módulo financeiro do contrato Nº {createInvoice.ContractReference}",
            TotalPrice = createInvoice.Price,
            Price = createInvoice.Price,
            DueDate = createInvoice.DueDate
        };
        await _invoiceRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        var externalId = await CreateInAssasAsync(record.Id, createInvoice.ExternalId, record.Description, createInvoice);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        record.ExternalId = externalId;
        _invoiceRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
