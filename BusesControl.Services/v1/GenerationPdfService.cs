using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Responses;
using BusesControl.Filters.Notification;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace BusesControl.Services.v1;

public class GenerationPdfService(
    INotificationApi _notificationApi
) : IGenerationPdfService
{
    private static string RenderTemplateContract(CustomerContractModel customerContract, string template)
    {
        var totalPriceCustomer = customerContract.Contract.TotalPrice / customerContract.Contract.CustomersCount;

        var terminationFee = customerContract.Contract.SettingPanel.TerminationFee;

        var placeholders = new Dictionary<string, string>
        {
            { "{{Title}}", customerContract.Contract.ContractDescription.Title },
            { "{{SubTitle}}", customerContract.Contract.ContractDescription.SubTitle },
            { "{{Owner}}", customerContract.Contract.ContractDescription.Owner },
            { "{{Objective}}", customerContract.Contract.ContractDescription.Objective },
            { "{{GeneralProvisions}}", customerContract.Contract.ContractDescription.GeneralProvisions },
            { "{{Copyright}}", $"&copy; {DateTime.UtcNow.Year} {customerContract.Contract.ContractDescription.Copyright}" },
            { "{{Contractor}}", RenderContractorForContractOrTermination(customerContract.Customer) },
            { "{{Bus}}", RenderBusForContract(customerContract.Contract.Bus) },
            { "{{Validity}}", RenderValidityForContract(customerContract.Contract.StartDate!.Value, customerContract.Contract.TerminateDate, customerContract.Contract.Reference) },
            { "{{Termination}}", RenderTerminationForContract(terminationFee, totalPriceCustomer) },
            { "{{Payments}}", RenderPaymentsForContract(customerContract.Contract.InstallmentsCount!.Value, totalPriceCustomer, customerContract.Contract.StartDate.Value.Day) },
            { "{{ContractorsObligations}}", RenderContractorsObligationsForContract(customerContract.Contract.SettingPanel.LateFeeInterestRate) }
        };

        foreach (var placeholder in placeholders)
        {
            var placeholderPattern = Regex.Escape(placeholder.Key);
            template = Regex.Replace(template, placeholderPattern, placeholder.Value);
        }

        return template;
    }

    private static string RenderTemplateTermination(CustomerContractModel customerContract, string template)
    {
        var totalPriceCustomer = customerContract.Contract.TotalPrice / customerContract.Contract.CustomersCount;

        var terminationFee = customerContract.Contract.SettingPanel.TerminationFee;

        var placeholders = new Dictionary<string, string>
        {
            { "{{Owner}}", customerContract.Contract.ContractDescription.Owner },
            { "{{Copyright}}", $"&copy; {DateTime.UtcNow.Year} {customerContract.Contract.ContractDescription.Copyright}" },
            { "{{ReferenceContract}}", RenderSubTitleForTermination(customerContract.Contract.Reference)},
            { "{{Contractor}}", RenderContractorForContractOrTermination(customerContract.Customer) },
            { "{{Termination}}", RenderTerminationForTermination(terminationFee, totalPriceCustomer, customerContract.Contract.Reference) }
        };

        foreach (var placeholder in placeholders)
        {
            var placeholderPattern = Regex.Escape(placeholder.Key);
            template = Regex.Replace(template, placeholderPattern, placeholder.Value);
        }

        return template;
    }

    private static string RenderSubTitleForTermination(string reference)
    {
        return $"{reference}";
    }

    private static string RenderTerminationForTermination(decimal terminationFee, decimal totalPrice, string reference)
    {
        decimal terminationAmount = totalPrice * terminationFee / 100;

        return $"\"Em caso de rescisão antecipada deste contrato sem o devido pagamento das parcelas, será aplicada uma multa de <span class=\"highlight\">{terminationFee}%</span> sobre o valor total do contrato por cliente. O valor a ser pago pela rescisão é de <span class=\"highlight\">{terminationAmount:C2}</span>.\" Em conformidade com as disposições legais e a cláusula de rescisão contratual, é obrigação do cliente efetuar o pagamento de {terminationAmount:C2} para validar a rescisão do contrato nº <span class=\"highlight\">{reference}</span>.";
    }

    private static string RenderContractorForContractOrTermination(CustomerModel customer)
    {
        if (customer.Type == CustomerTypeEnum.NaturalPerson)
        {
            string cpf = Convert.ToUInt64(customer.Cpf).ToString(@"000\.000\.000\-00");

            return $"<span class=\"highlight\">{customer.Name}</span>, portador(a) do cpf nº <span class=\"highlight\">{cpf}</span>, residente à {customer.HomeNumber}, próximo ao {customer.ComplementResidential}, no bairro {customer.Neighborhood}, na cidade de {customer.City} - {customer.State}, neste ato qualificado(a) como <span class=\"highlight\">contratante</span>. Se necessário, contatar através dos seguintes meios: <span class=\"highlight\">{Convert.ToInt64(customer.PhoneNumber):(##) #####-####}</span> ou <span class=\"highlight\">{customer.Email}</span>.";
        }
        else
        {
            string cnpj = Convert.ToUInt64(customer.Cnpj).ToString(@"00\.000\.000\/0000\-00");

            return $"<span class=\"highlight\">{customer.Name}</span>, inscrito(a) no CNPJ sob o nº <span class=\"highlight\">{cnpj}</span>, com sede à {customer.HomeNumber}, próximo ao {customer.ComplementResidential}, no bairro {customer.Neighborhood}, na cidade de {customer.City} - {customer.State}, neste ato representado(a) por seu(sua) representante legal, doravante denominado(a) <span class=\"highlight\">contratante</span>. Se necessário, contatar através dos seguintes meios: <span class=\"highlight\">{Convert.ToInt64(customer.PhoneNumber):(##) #####-####}</span> ou <span class=\"highlight\">{customer.Email}</span>.";
        }
    }

    private static string RenderBusForContract(BusModel bus)
    {
        var licensePlate = $"{bus.LicensePlate.Substring(0, 3).ToUpper()}-{bus.LicensePlate.Substring(3)}";

        return $"Veículo {bus.Brand}, modelo {bus.Name}, placa <span class=\"highlight\">{licensePlate}</span>, " +
               $"renavam {bus.Renavam}, chassi <span class=\"highlight\">{bus.Chassi}</span>, cor {bus.Color.Color}, fabricado em {bus.ManufactureDate.ToString("dd 'de' MMMM 'de' yyyy")}, " +
               $"com capacidade para {bus.SeatingCapacity} passageiros.";
    }

    private static string RenderValidityForContract(DateOnly initialDate, DateOnly terminationDate, string reference)
    {
        return $"O presente contrato nº <span class=\"highlight\">{reference}</span> vigorará pelo período de <span class=\"highlight\">{initialDate.ToString("dd 'de' MMMM 'de' yyyy")}</span> a <span class=\"highlight\">{terminationDate.ToString("dd 'de' MMMM 'de' yyyy")}</span>, conforme acordado entre as partes.";
    }

    private static string RenderTerminationForContract(decimal terminationFee, decimal totalPrice)
    {
        decimal terminationAmount = totalPrice * terminationFee / 100;

        return $"Em caso de rescisão antecipada deste contrato sem o devido pagamento das parcelas, será aplicada uma multa de <span class=\"highlight\">{terminationFee}%</span> sobre o valor total do contrato por cliente. O valor a ser pago pela rescisão é de <span class=\"highlight\">{terminationAmount:C2}</span>.";
    }

    private static string RenderPaymentsForContract(int installmentsCount, decimal totalPriceCustomer, int day)
    {
        var paymentPortion = totalPriceCustomer / installmentsCount;

        return @$"Os serviços serão remunerados pela Contratante à Contratada no valor total " +
                $"de <span class=\"highlight\">{totalPriceCustomer:C2}</span>,\r\n            " +
                $"pagos em {installmentsCount} parcelas mensais de <span class=\"highlight\">{paymentPortion:C2}</span>. " +
                $"O vencimento ocorrerá no dia {day} de cada mês, \r\n observando as variações no número de dias entre os meses.";
    }

    private static string RenderContractorsObligationsForContract(decimal lateFeeRate)
    {
        string baseClause = $"O Contratante se compromete a cumprir rigorosamente as datas de pagamento estipuladas neste contrato,";

        if (lateFeeRate > 0)
        {
            baseClause += $" sob pena de aplicação de juros moratórios de <span class=\"highlight\">{lateFeeRate}% ao mês</span> sobre as parcelas em atraso.";
        }

        baseClause += " O serviço prestado poderá ser suspenso em caso de atraso, ficando a critério da prestadora.";

        return baseClause;
    }

    public async Task<PdfCoResponse> GeneratePdfFromTemplateAsync(CustomerContractModel customerContract)
    {
        var basePath = AppContext.BaseDirectory;
        var templatePath = Path.Combine(basePath, "..", "..", "..", "..", "BusesControl.Services", "v1", "Templates", "TemplateContract.html");
        var template = File.ReadAllText(templatePath);

        var fileName = $"Contrato - {customerContract.Customer.Name}";
        var renderedHtml = RenderTemplateContract(customerContract, template);

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("x-api-key", AppSettingsPdfCo.Key);

        var requestContent = new MultipartFormDataContent
        {
            { new StringContent(fileName), "name" },
            { new StringContent(renderedHtml), "html" }
        };

        var httpResult = await httpClient.PostAsync(AppSettingsPdfCo.Url, requestContent);
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.CustomerContract.PdfUnexpected
            );
            return default!;
        }

        var response = await httpResult.Content.ReadFromJsonAsync<PdfCoResponse>();
        if (response is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.CustomerContract.PdfUnexpected
            );
            return default!;
        }

        return response;
    }

    public async Task<PdfCoResponse> GeneratePdfTerminationFromTemplateAsync(CustomerContractModel customerContract)
    {
        var basePath = AppContext.BaseDirectory;
        var templatePath = Path.Combine(basePath, "..", "..", "..", "..", "BusesControl.Services", "v1", "Templates", "TemplateTerminationContract.html");
        var template = File.ReadAllText(templatePath);

        var fileName = $"Rescisão - {customerContract.Customer.Name}";
        var renderedHtml = RenderTemplateTermination(customerContract, template);

        var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("x-api-key", AppSettingsPdfCo.Key);

        var bodyRequest = new MultipartFormDataContent
        {
            { new StringContent(fileName), "name" },
            { new StringContent(renderedHtml), "html" }
        };

        var httpResult = await httpClient.PostAsync(AppSettingsPdfCo.Url, bodyRequest);
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.CustomerContract.PdfUnexpected
            );
            return default!;
        }

        var response = await httpResult.Content.ReadFromJsonAsync<PdfCoResponse>();
        if (response is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.CustomerContract.PdfUnexpected
            );
            return default!;
        }

        return response;
    }
}
