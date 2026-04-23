using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace BusesControl.Services.v1
{
    public class AsaasService : IAsaasService
    {
        public AsaasService(IOptions<AppSettings> options, INotificationContext notificationContext, ISavedCardService savedCardService)
        {
            _appSettings = options.Value;
            _notificationContext = notificationContext;
            _savedCardService = savedCardService;
        }

        private readonly AppSettings _appSettings;
        private readonly INotificationContext _notificationContext;
        private readonly ISavedCardService _savedCardService;

        public async Task<string> CreateCustomerAsync(CustomerModel customer)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

            var createCustomerInAssas = new
            {
                name = customer.Name,
                cpfCnpj = customer.Cpf ?? customer.Cnpj,
                mobilePhone = customer.PhoneNumber,
                email = customer.Email
            };

            var httpResult = await httpClient.PostAsJsonAsync($"{_appSettings.Assas.Url}/customers", createCustomerInAssas);
            if (!httpResult.IsSuccessStatusCode)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Customer.Unexpected
                );
                return default!;
            }

            var customerExternal = await httpResult.Content.ReadFromJsonAsync<CreateCustomerInAssasDTO>();
            if (customerExternal is null)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Customer.Unexpected
                );
                return default!;
            }

            return customerExternal.Id;
        }

        public async Task<bool> UpdateCustomerAsync(string externalId, CustomerUpdateRequest customer)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

            var updateCustomerInAssas = new
            {
                name = customer.Name,
                cpfCnpj = customer.Cpf ?? customer.Cnpj,
                mobilePhone = customer.PhoneNumber,
                email = customer.Email
            };

            var httpResult = await httpClient.PutAsJsonAsync($"{_appSettings.Assas.Url}/customers/{externalId}", updateCustomerInAssas);
            if (!httpResult.IsSuccessStatusCode)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Customer.Unexpected
                );
                return false;
            }

            return true;
        }

        public async Task<string> CreatePaymentAsync(Guid id, string externalId, string descriptionInvoice, CreateInvoiceDTO createInvoice)
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
                externalReference = id,
                fine = new
                {
                    type = "PERCENTAGE"
                }
            };

            var httpResult = await httpClient.PostAsJsonAsync($"{_appSettings.Assas.Url}/payments", createInvoiceInAssas);
            if (!httpResult.IsSuccessStatusCode)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Invoice.UnexpectedCreate
                );
                return default!;
            }

            var invoiceExternal = await httpResult.Content.ReadFromJsonAsync<CreateInvoiceInAssasDTO>();
            if (invoiceExternal is null)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Invoice.UnexpectedCreate
                );
                return default!;
            }

            return invoiceExternal.Id;
        }

        public async Task<bool> UpdatePaymentAsync(InvoiceModel updateInvoice, decimal interest = 0)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

            var invoiceUpdateInAssas = new
            {
                billingType = "UNDEFINED",
                dueDate = updateInvoice.DueDate,
                value = updateInvoice.Price,
                description = updateInvoice.Description,
                externalReference = updateInvoice.Id,
                interest = new
                {
                    value = interest
                }
            };

            var httpResult = await httpClient.PutAsJsonAsync($"{_appSettings.Assas.Url}/payments/{updateInvoice.ExternalId}", invoiceUpdateInAssas);
            if (!httpResult.IsSuccessStatusCode)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Invoice.Unexpected
                );
                return false;
            }

            var response = await httpResult.Content.ReadAsStringAsync();
            if (response is null)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Invoice.Unexpected
                );
                return false;
            }

            return true;
        }

        public async Task<bool> RemovePaymentAsync(string externalId)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

            var httpResult = await httpClient.DeleteAsync($"{_appSettings.Assas.Url}/payments/{externalId}");
            if (!httpResult.IsSuccessStatusCode)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Invoice.Unexpected
                );
                return false;
            }

            var response = await httpResult.Content.ReadAsStringAsync();
            if (response is null)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Invoice.Unexpected
                );
                return false;
            }

            return true;
        }

        public async Task<InvoicePayWithCardInAssasDTO> CreditCardPaymentAsync(InvoiceModel record, InvoicePaymentRequest request)
        {
            if (request.CreditCard is null)
            {
                _notificationContext.SetNotification(
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
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Invoice.UnexpectedPay
                );
                return default!;
            }

            var response = await httpResult.Content.ReadFromJsonAsync<InvoicePayWithCardInAssasDTO>();
            if (response!.Status != "CONFIRMED" && response.Status != "RECEIVED")
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Invoice.UnexpectedPay
                );
                return default!;
            }

            await _savedCardService.CreateAsync(record.Financial.CustomerId!.Value, response.CreditCard.CreditCardNumber, response.CreditCard.CreditCardBrand, Guid.Parse(response.CreditCard.CreditCardToken));

            return response;
        }

        public async Task<PaymentPixResponse> PixPaymentAsync(InvoiceModel record)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

            var httpResult = await httpClient.GetAsync($"{_appSettings.Assas.Url}/payments/{record.ExternalId}/pixQrCode");
            if (!httpResult.IsSuccessStatusCode)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Invoice.UnexpectedPix
                );
                return default!;
            }

            var response = await httpResult.Content.ReadFromJsonAsync<PaymentPixResponse>();
            if (response is null)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Invoice.UnexpectedPix
                );
                return default!;
            }

            return response;
        }
    }
}