using AutoMapper;
using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Response;
using BusesControl.Entities.Responses;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BusesControl.Services.v1;

public class WebhookService(
    AppSettings _appSettings,
    IMapper _mapper,
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    INotificationService _notificationService,
    IInvoiceBusiness _invoiceBusiness,
    IWebhookBusiness _webhookBusiness,
    IInvoiceRepository _invoiceRepository,
    IWebhookRepository _webhookRepository
) : IWebhookService
{
    private async Task<string> CreateInAssasAsync(WebhookModel record)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var webhookInAssas = new 
        { 
            name = record.Name,
            email = record.Email,
            url = record.Url,
            sendType = record.SendType,
            enabled = record.Enabled,
            interrupted = record.Interrupted,
            apiVersion = record.ApiVersion,
            authToken = record.AuthToken,
            events = JsonSerializer.Deserialize<IEnumerable<string>>(record.Events),
        };

        var httpResult = await httpClient.PostAsJsonAsync($"{_appSettings.Assas.Url}/webhooks", webhookInAssas);
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Webhook.Unexpected
            );
            return default!;
        }

        var webhookExternal = await httpResult.Content.ReadFromJsonAsync<CreateWebhookInAssasDTO>();
        if (webhookExternal is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Webhook.Unexpected
            );
            return default!;
        }

        return webhookExternal.Id;
    }

    private async Task<bool> DeleteInAssasAsync(string externalId)
    {
        var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var httpResult = await httpClient.DeleteAsync($"{_appSettings.Assas.Url}/webhooks/{externalId}");
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Webhook.Unexpected
            );
            return false;
        }

        return true;
    }

    public async Task<IEnumerable<WebhookResponse>> GetAllAsync()
    {
        var records = await _webhookRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<WebhookResponse>>(records);
    }

    public async Task<WebhookResponse> GetByIdAsync(Guid id)
    {
        var record = await _webhookRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Webhook.NotFound
            );
            return default!;
        }

        return _mapper.Map<WebhookResponse>(record);
    }

    public async Task<bool> CreateAsync(WebhookCreateRequest request)
    {
        var exists = await _webhookRepository.ExistsByNameOrUrlOrTypeAsync(request.Name, request.Url, request.Type);
        if (exists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.Webhook.ExistsByNameOrUrlOrType
            );
            return false;
        }

        var record = new WebhookModel
        {
            Name = request.Name,
            Url = request.Url,
            Email = request.Email,
            Type = request.Type,
            AuthToken = TokenGenerator.GenerateToken(64),
            SendType = _appSettings.WebhookAssas.SendType,
            Enabled = _appSettings.WebhookAssas.Enabled,
            Interrupted = _appSettings.WebhookAssas.Interrupted,
            ApiVersion = _appSettings.WebhookAssas.ApiVersion,
            Events = JsonSerializer.Serialize(request.Events)
        };

        record.ExternalId = await CreateInAssasAsync(record);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await _webhookRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<WebhookChangeTokenResponse> ChangeWebhookAsync(WebhookModel record)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var webhookInAssas = new
        {
            enabled = true,
            authToken = TokenGenerator.GenerateToken(64),
        };

        var httpResult = await httpClient.PutAsJsonAsync($"{_appSettings.Assas.Url}/webhooks/{record.ExternalId}", webhookInAssas);
        if (!httpResult.IsSuccessStatusCode)
        {
            return new WebhookChangeTokenResponse(false, Message.Webhook.Unexpected);
        }

        var webhookExternal = await httpResult.Content.ReadFromJsonAsync<CreateWebhookInAssasDTO>();
        if (webhookExternal is null)
        {
            return new WebhookChangeTokenResponse(false, Message.Webhook.Unexpected);
        }

        record.UpdatedAt = DateTime.UtcNow;
        record.Enabled = webhookInAssas.enabled;
        record.AuthToken = webhookInAssas.authToken;

        _webhookRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return new WebhookChangeTokenResponse(success: true);
    }

    public async Task<SuccessResponse> DeleteAsync(Guid id)
    {
        var record = await _webhookRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Webhook.NotFound
            );
            return default!;
        }

        await DeleteInAssasAsync(record.ExternalId);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        _webhookRepository.Delete(record);
        await _unitOfWork.CommitAsync();

        return new SuccessResponse(Message.Webhook.SuccessDelete);
    }

    public async Task<bool> PaymentPixAsync(string? accessToken, PaymentPixRequest request)
    {
        await _webhookBusiness.ValidateTokenAsync(authToken: accessToken, type: WebhookTypeEnum.Received);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        if (request.Payment is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Webhook.RequestRequired
            );
            return false;
        }

        if (request.Event != "PAYMENT_RECEIVED")
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Webhook.EventNotAccepted
            );
            return false;
        }

        var record = await _invoiceBusiness.GetForPaymentPixAsync(request.Payment.ExternalReference, request.Payment.Id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        _unitOfWork.BeginTransaction();

        record.UpdatedAt = DateTime.UtcNow;
        record.PaymentDate = request.Payment.PaymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        record.Status = InvoiceStatusEnum.Paid;
        _invoiceRepository.Update(record);
        await _unitOfWork.CommitAsync();

        await _notificationService.SendInternalNotificationAsync(TemplateTitle.InvoicePaymentPix, TemplateMessage.InvoicePaymentPix(record.Reference), NotificationAccessLevelEnum.Public);

        await _unitOfWork.CommitAsync(true);

        return true;
    }
}
