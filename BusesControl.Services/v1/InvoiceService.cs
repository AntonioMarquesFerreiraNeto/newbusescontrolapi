using Azure;
using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Responses;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace BusesControl.Services.v1;

public class InvoiceService(
    AppSettings _appSettings,
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    ISavedCardService _savedCardService,
    IInvoiceBusiness _invoiceBusiness,
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
        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var createInvoiceInAssas = new
        {
            billingType = "UNDEFINED",
            customer = externalId,
            dueDate = createInvoice.DueDate,
            value = createInvoice.Price,
            description = descriptionInvoice,
            externalReference = id
        };

        var httpResult = await httpClient.PostAsJsonAsync($"{_appSettings.Assas.Url}/payments", createInvoiceInAssas);
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.UnexpectedCreate
            );
            return default!;
        }

        var invoiceExternal = await httpResult.Content.ReadFromJsonAsync<CreateInvoiceInAssasDTO>();
        if (invoiceExternal is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.UnexpectedCreate
            );
            return default!;
        }

        return invoiceExternal.Id;
    }

    private async Task<InvoicePayWithCardInAssasDTO> HandleCreditCardPaymentAsync(InvoiceModel record, InvoicePaymentRequest request)
    {
        if (request.CreditCard is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.NotCreditCard
            );
            return default!;
        }

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var invoicePaymentInAssas = new
        {
            creditCard = new
            {
                holderName = request.CreditCard!.HolderName,
                number = request.CreditCard!.Number,
                expiryMonth = request.CreditCard.ExpiryMonth,
                expiryYear = request.CreditCard.ExpiryYear,
                ccv = request.CreditCard.SecurityCode
            },
            creditCardHolderInfo = new
            {
                name = request.CreditCard!.HolderName,
                cpfCnpj = request.CreditCard.HolderCpfCnpj,
                email = request.CreditCard.HolderEmail,
                mobilePhone = request.CreditCard.HolderMobilePhone,
                postalCode = request.CreditCard.HolderPostalCode,
                addressNumber = request.CreditCard.HolderAddressNumber
            }
        };

        var httpResult = await httpClient.PostAsJsonAsync($"{_appSettings.Assas.Url}/payments/{record.ExternalId}/payWithCreditCard", invoicePaymentInAssas);
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.UnexpectedPay
            );
            return default!;
        }

        var response = await httpResult.Content.ReadFromJsonAsync<InvoicePayWithCardInAssasDTO>();
        if (response!.Status != "CONFIRMED" && response.Status != "RECEIVED")
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.UnexpectedPay
            );
            return default!;
        }

        if (record.Financial.Type == FinancialTypeEnum.Expense)
        {
            await _savedCardService.CreateAsync(record.Financial.CustomerId!.Value, response.CreditCard.CreditCardNumber, response.CreditCard.CreditCardBrand, Guid.Parse(response.CreditCard.CreditCardToken));
        }

        return response;
    }

    private async Task<PaymentPixResponse> HandlePixPaymentAsync(InvoiceModel record)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var httpResult = await httpClient.GetAsync($"{_appSettings.Assas.Url}/payments/{record.ExternalId}/pixQrCode");
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.UnexpectedPix
            );
            return default!;
        }

        var response = await httpResult.Content.ReadFromJsonAsync<PaymentPixResponse>();
        if (response is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.UnexpectedPix
            );
            return default!;
        }

        return response;
    }

    private async Task<bool> RemoveInAssasAsync(string externalId)
    {
        var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var httpResult = await httpClient.DeleteAsync($"{_appSettings.Assas.Url}/payments/{externalId}");
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.UnexpectedRemove
            );
            return false;
        }

        var response = await httpResult.Content.ReadAsStringAsync();
        if (response is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.UnexpectedRemove
            );
            return false;
        }

        return true;
    }

    public async Task<bool> CreateForFinancialAsync(CreateInvoiceDTO createInvoice)
    {
        createInvoice.SetTitleAndDescription();

        var record = new InvoiceModel
        {
            Reference = await GenerateReferenceUniqueAsync(),
            FinancialId = createInvoice.FinancialId,
            Title = createInvoice.Title,
            Description = createInvoice.Description,
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

    public async Task<InvoicePaymentResponse> PaymentAsync(Guid id, InvoicePaymentRequest request)
    {
        var record = await _invoiceBusiness.GetForPaymentAsync(id);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        var invoicePayWithCardResponse = new InvoicePayWithCardInAssasDTO();
        var invoicePaymentResponse = new InvoicePaymentResponse();

        _unitOfWork.BeginTransaction();

        switch (request.PaymentMethod)
        {
            case PaymentMethodEnum.CreditCard:
            {
                invoicePayWithCardResponse = await HandleCreditCardPaymentAsync(record, request);
                if (_notificationApi.HasNotification)
                {
                    return default!;
                }

                invoicePaymentResponse.Message = Message.Invoice.SuccessPay;
            }
            break;

            case PaymentMethodEnum.Pix:
            {
                invoicePaymentResponse.Pix = await HandlePixPaymentAsync(record);
                if (_notificationApi.HasNotification)
                {
                    return default!;
                }

                invoicePaymentResponse.Message = Message.Invoice.SuccessPix;
            }
            break;
        }

        if (request.PaymentMethod == PaymentMethodEnum.CreditCard)
        {
            record.PaymentDate = invoicePayWithCardResponse.ConfirmedDate;
            record.UpdatedAt = DateTime.UtcNow;
            record.Status = InvoiceStatusEnum.Paid;
            _invoiceRepository.Update(record);
            await _unitOfWork.CommitAsync();
        }

        await _unitOfWork.CommitAsync(true);

        return invoicePaymentResponse;
    }

    public async Task<AutomatedPaymentResponse> AutomatedPaymentAsync(InvoiceModel record, Guid creditCardToken)
    {
        var httpCliente = new HttpClient();
        httpCliente.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var automatedPayment = new 
        {
            creditCardToken,
        };

        var httpResult = await httpCliente.PostAsJsonAsync($"{_appSettings.Assas.Url}/payments/{record.ExternalId}/payWithCreditCard", automatedPayment);
        if (!httpResult.IsSuccessStatusCode)
        {
            var assasErrorResponse = await httpResult.Content.ReadFromJsonAsync<AssasErrorResponseDTO>();
            return new AutomatedPaymentResponse(messageFailure: assasErrorResponse?.Errors.First().Description);
        }

        var response = await httpResult.Content.ReadFromJsonAsync<InvoicePayWithCardInAssasDTO>();
        if (response!.Status != "CONFIRMED" && response.Status != "RECEIVED")
        {
            return new AutomatedPaymentResponse(messageFailure: Message.Invoice.FailureAutomatedPay);
        }

        record.PaymentDate = response.ConfirmedDate;
        record.UpdatedAt = DateTime.UtcNow;
        record.Status = InvoiceStatusEnum.Paid;
        _invoiceRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return new AutomatedPaymentResponse(sucess: true);
    }

    public async Task<(bool success, string? errorMessage)> ChangeOverDueForSystemAsync(InvoiceModel record)
    {
        if (record.Status != InvoiceStatusEnum.Pending)
        {
            return (false, Message.Invoice.FailureOverDue);
        }

        record.UpdatedAt = DateTime.UtcNow;
        record.Status = InvoiceStatusEnum.OverDue;
        _invoiceRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return (true, null);
    }

    public async Task<bool> CancelForFinancialAsync(InvoiceModel record)
    {
        await RemoveInAssasAsync(record.ExternalId!);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        record.UpdatedAt = DateTime.UtcNow;
        record.Status = InvoiceStatusEnum.Canceled;
        _invoiceRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
