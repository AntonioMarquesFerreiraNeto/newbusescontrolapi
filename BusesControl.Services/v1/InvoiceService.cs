using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace BusesControl.Services.v1;

public class InvoiceService(
    AppSettings _appSettings,
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    IUserService _userService,
    IInvoiceBusiness _invoiceBusiness,
    IInvoiceRepository _invoiceRepository,
    IAsaasService _asaasService
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

    public async Task<bool> CreateInternalAsync(CreateInvoiceDTO createInvoice)
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
        await _invoiceRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        var externalId = await _asaasService.CreatePaymentAsync(record.Id, createInvoice.CustomerExternalId, record.Description, createInvoice);
        if (_notificationContext.HasNotification)
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
        if (_notificationContext.HasNotification)
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
                invoicePayWithCardResponse = await _asaasService.CreditCardPaymentAsync(record, request);
                if (_notificationContext.HasNotification)
                {
                    return default!;
                }

                invoicePaymentResponse.Message = Message.Invoice.SuccessPay;
            }
            break;

            case PaymentMethodEnum.Pix:
            {
                invoicePaymentResponse.Pix = await _asaasService.PixPaymentAsync(record);
                if (_notificationContext.HasNotification)
                {
                    return default!;
                }

                invoicePaymentResponse.Message = Message.Invoice.SuccessPix;
            }
            break;

            case PaymentMethodEnum.JustCount:
            {
                _invoiceBusiness.ValidateLoggedUserForJustCountPayment(_userService.FindAuthenticatedUser());
                if (_notificationContext.HasNotification)
                {
                    return default!;
                }

                invoicePaymentResponse.Message = Message.Invoice.SuccessJustCount;
            }
            break;
        }

        if (request.PaymentMethod == PaymentMethodEnum.CreditCard || request.PaymentMethod == PaymentMethodEnum.JustCount)
        {
            record.PaymentDate = invoicePayWithCardResponse.ConfirmedDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
            record.UpdatedAt = DateTime.UtcNow;
            record.PaymentMethod = request.PaymentMethod;
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
        record.PaymentMethod = PaymentMethodEnum.CreditCard;
        record.Status = InvoiceStatusEnum.Paid;
        _invoiceRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return new AutomatedPaymentResponse(sucess: true);
    }

    public async Task<(bool success, string? errorMessage)> ChangeOverDueInternalAsync(InvoiceModel record)
    {
        if (record.Status != InvoiceStatusEnum.Pending)
        {
            return (false, Message.Invoice.FailureOverDue);
        }

        if (record.Financial.SettingPanel is null)
        {
            return (false, Message.Invoice.SettingPanelNotFound);
        }

        var lateFeeInterestRate = record.Financial.SettingPanel.LateFeeInterestRate;

        if (lateFeeInterestRate >= 1)
        {
            record.InterestRate = Math.Round(record.Price * lateFeeInterestRate / 100, 2);
            record.TotalPrice = Math.Round(record.TotalPrice + record.InterestRate, 2);

            await _asaasService.UpdatePaymentAsync(record, interest: lateFeeInterestRate);
            if (_notificationContext.HasNotification)
            {
                string errorMessage = _notificationContext.Details;
                _notificationContext.Reset();

                return (false, errorMessage);
            }
        }

        record.Financial.SettingPanel = null;

        record.UpdatedAt = DateTime.UtcNow;
        record.Status = InvoiceStatusEnum.OverDue;
        _invoiceRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return (true, null);
    }

    public async Task<bool> CancelInternalAsync(InvoiceModel record)
    {
        await _asaasService.RemovePaymentAsync(record.ExternalId!);
        if (_notificationContext.HasNotification)
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
