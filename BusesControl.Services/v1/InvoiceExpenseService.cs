﻿using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace BusesControl.Services.v1;

public class InvoiceExpenseService(
    AppSettings _appSettings,
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    IUserService _userService,
    IInvoiceExpenseBusiness _invoiceExpenseBusiness,
    IInvoiceExpenseRepository _invoiceExpenseRepository
) : IInvoiceExpenseService
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
            existsReference = await _invoiceExpenseRepository.ExistsByReferenceAsync(reference);
        }

        return reference;
    }

    private async Task<string> HandlePaymentPixAsync(InvoiceExpenseModel record, InvoiceExpensePixRequest request)
    {
        //TODO: TESTAR E FACILITAR O BODY DA REQUEST APÓS APROVAÇÃO DESSE RECURSO NO ASSAS.
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var paymentTedInAssas = new
        {
            value = record.TotalPrice,
            operationType = "PIX",
            description = record.Description,
            bankAccount = new
            {
                bank = new
                {
                    code = request.BankCode
                },
                accountName = request.AccountName,
                ownerName = request.OwnerName,
                ownerBirthDate = request.OwnerBirthDate,
                cpfCnpj = request.CpfCnpj,
                bankAccountType = request.CurrentAccount ? "CONTA_CORRENTE" : "CONTA_POUPANCA",
                account = request.Account,
                accountDigit = request.AccountDigit,
                agency = request.Agency
            }
        };

        var httpResult = await httpClient.PostAsJsonAsync($"{_appSettings.Assas.Url}/transfers", paymentTedInAssas);
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.InvoiceExpense.UnexpectedPay
            );
            return default!;
        }

        var response = await httpResult.Content.ReadFromJsonAsync<PaymentInvoiceExpenseInAssasDTO>();
        if (response is null || response.Status == "CANCELLED" || response.Status == "FAILED")
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.InvoiceExpense.UnexpectedPay
            );
            return default!;
        }

        return response.Id;
    }

    public async Task<bool> CreateInternalAsync(CreateInvoiceExpenseInDTO createInvoiceExpense)
    {
        createInvoiceExpense.SetTitleAndDescription();

        var record = new InvoiceExpenseModel
        {
            Reference = await GenerateReferenceUniqueAsync(),
            FinancialId = createInvoiceExpense.FinancialId,
            Title = createInvoiceExpense.Title,
            Description = createInvoiceExpense.Description,
            Price = createInvoiceExpense.Price,
            TotalPrice = createInvoiceExpense.Price,
            DueDate = createInvoiceExpense.DueDate
        };
        await _invoiceExpenseRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> CancelInternalAsync(IEnumerable<InvoiceExpenseModel> records)
    {
        records = records.Where(x => x.Status == InvoiceExpenseStatusEnum.Pending).Select(invoice => 
        {
            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.Status = InvoiceExpenseStatusEnum.Canceled; 
            return invoice;
        });
        _invoiceExpenseRepository.UpdateRange(records);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<SuccessResponse> PaymentAsync(Guid id, InvoiceExpensePaymentRequest request)
    {
        var successResponse = new SuccessResponse("");
        string? externalId = null;

        var record = await _invoiceExpenseBusiness.GetForPaymentAsync(id);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        if (request.PaymentMethod != PaymentExpenseMethodEnum.JustCount)
        {
            await _invoiceExpenseBusiness.ValidateBalanceInAssasAsync(record.TotalPrice);
            if (_notificationContext.HasNotification)
            {
                return default!;
            }
        }

        switch (request.PaymentMethod)
        {
            case PaymentExpenseMethodEnum.Pix:
            {
                externalId = await HandlePaymentPixAsync(record, request.PixRequest!);
                if (_notificationContext.HasNotification)
                {
                    return default!;
                }

                successResponse.Message = Message.InvoiceExpense.SuccessPayPix;
            }
            break;

            case PaymentExpenseMethodEnum.JustCount:
            {
                _invoiceExpenseBusiness.ValidateLoggedUserForJustCountPayment(_userService.FindAuthenticatedUser());
                if (_notificationContext.HasNotification)
                {
                    return default!;
                }

                successResponse.Message = Message.InvoiceExpense.SuccessPayJustCount;
            }
            break;
        }

        record.UpdatedAt = DateTime.UtcNow;
        record.PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        record.PaymentMethod = request.PaymentMethod;
        record.ExternalId = externalId;
        record.Status = InvoiceExpenseStatusEnum.Paid;
        _invoiceExpenseRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return successResponse;
    }
}
