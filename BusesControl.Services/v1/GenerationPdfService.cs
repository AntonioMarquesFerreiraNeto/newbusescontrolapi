using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
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
    private string RenderTemplate(CustomerContractModel record, string template)
    {
        var placeholders = new Dictionary<string, string>
        {
            { "{Contractor}", RenderContractor(record.Customer) },
            { "{Bus}", RenderBus(record.Contract.Bus) },
            { "{Validity}", RenderValidity(record.Contract.StartDate ?? record.Contract.CreatedAt, record.Contract.TerminateDate) },
            { "{Termination}", RenderTermination(record.Contract.TotalPrice) }
        };

        foreach (var placeholder in placeholders)
        {
            var placeholderPattern = Regex.Escape(placeholder.Key);
            template = Regex.Replace(template, placeholderPattern, placeholder.Value);
        }

        return template;
    }

    private string RenderContractor(CustomerModel customer)
    {
        string cpfOrCnpj = customer.Type == Entities.Enums.CustomerTypeEnum.NaturalPerson
                            ? Convert.ToUInt64(customer.Cpf).ToString(@"000\.000\.000\-00")
                            : Convert.ToUInt64(customer.Cnpj).ToString(@"00\.000\.000\/0000\-00");

        return $"<span class=\"highlight\">{customer.Name}</span>, portador(a) do CPF: <span class=\"highlight\">{cpfOrCnpj}</span>, " +
               $"residente na residência {customer.HomeNumber}, próximo ao complemento residencial {customer.ComplementResidential}, " +
               $"no bairro {customer.Neighborhood}, em {customer.City} - {customer.State}, neste ato qualificado(a) como <span class=\"highlight\">contratante</span>.";
    }

    private string RenderBus(BusModel bus)
    {
        var licensePlate = $"{bus.LicensePlate.Substring(0, 3).ToUpper()}-{bus.LicensePlate.Substring(3)}";

        return $"Veículo {bus.Brand}, modelo {bus.Name}, placa <span class=\"highlight\">{licensePlate}</span>, " +
               $"chassi <span class=\"highlight\">{bus.Chassi}</span>, cor {bus.Color.Color}, fabricado em {bus.ManufactureDate.ToString("dd 'de' MMMM 'de' yyyy")}, " +
               $"com capacidade para {bus.SeatingCapacity} passageiros.";
    }

    private string RenderValidity(DateTime initialDate, DateTime terminationDate)
    {
        return $"O presente contrato vigorará pelo período de <span class=\"highlight\">{initialDate.ToString("dd 'de' MMMM 'de' yyyy")}</span> a <span class=\"highlight\">{terminationDate.ToString("dd 'de' MMMM 'de' yyyy")}</span>,\r\n                conforme acordado entre as partes.";
    }

    private string RenderTermination(decimal price)
    {
        return @$"Em caso de rescisão antecipada deste contrato sem o devido pagamento das parcelas, será aplicada uma multa de <span class=""highlight"">3%</span> sobre o valor total do contrato por cliente. A base de cálculo inicial é o valor total do contrato, que é <span class=""highlight"">R$ {price:C2}</span>, dividido pela quantidade de clientes no contrato.";
    }

    public async Task<PdfCoResponse> GeneratePdfFromTemplateAsync(CustomerContractModel record)
    {
        var basePath = AppContext.BaseDirectory;
        var templatePath = Path.Combine(basePath, "..", "..", "..", "..", "BusesControl.Services", "v1", "Templates", "TemplateContract.html");
        var template = File.ReadAllText(templatePath);

        var fileName = $"Contrato - {record.Customer.Name}";
        var renderedHtml = RenderTemplate(record, template);

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
}
