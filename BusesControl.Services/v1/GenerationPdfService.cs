﻿using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Services.v1.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class GenerationPdfService(
    IConverter _converter,
    INotificationApi _notificationApi,
    ICustomerContractRepository _customerContractRepository
) : IGenerationPdfService
{
    public async Task<byte[]> ContractForCustomerAsync(Guid contractId, Guid customerId)
    {
        var record = await _customerContractRepository.GetByContractAndCustomerWithIncludesAsync(contractId, customerId);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.CustomerContract.NotFound
            );
            return default!;
        }

        string htmlContent = GetHtmlContractTemplate(record);
        var doc = new HtmlToPdfDocument()
        { 
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            },
            Objects =
            {
                new ObjectSettings()
                {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
        };

        return _converter.Convert(doc);
    }

    //TODO: Migrar o html e css para um arquivo .html para o template ficar isolado em uma pasta de templates, e não harded code.
    // Mas antes é necessário deixar ele dinâmico de acordo com as informações do contrato, cliente, motorista e assim por diante.
    private string GetHtmlContractTemplate(CustomerContractModel record)
    {
        return @"
    <!DOCTYPE html>
    <html lang=""pt-BR"">
    <head>
        <meta charset=""UTF-8"">
        <title>Contrato de Prestação de Serviços</title>
        <style>
            body {
                font-family: 'Arial', sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f7f9fc;
                color: #333;
            }
            .container {
                width: 80%;
                margin: 30px auto;
                padding: 30px;
                background-color: #fff;
                box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
                border-radius: 10px;
            }
            h1, h2 {
                font-family: 'Poppins', sans-serif;
                text-align: center;
                color: #003366;
                margin-bottom: 20px;
            }
            p {
                margin: 15px 0;
                line-height: 1.8;
                text-align: justify;
            }
            .section-title {
                font-family: 'Poppins', sans-serif;
                font-weight: bold;
                color: #2b5787;
                margin-top: 30px;
                border-bottom: 1px solid #2b5787;
                padding-bottom: 8px;
            }
            .highlight {
                color: #b05d5a;
                font-weight: bold;
            }
            .signature-section {
                display: flex;
                justify-content: space-around;
                margin-top: 50px;
            }
            .signature-box {
                width: 30%;
                text-align: center;
            }
            .signature-line {
                margin-top: 40px;
                border-top: 1px solid #333;
                padding-top: 5px;
            }
            .footer {
                display: flex;
                justify-content: space-between;
                text-align: center;
                margin-top: 60px;
                font-size: 0.77em;
                color: #777;
            }
        </style>
        <link href=""https://fonts.googleapis.com/css2?family=Poppins:wght@400;700&display=swap"" rel=""stylesheet"">
    </head>
    <body>
        <div class=""container"">
            <h1>Contrato de Prestação de Serviços</h1>
            <h2>Transporte Rodoviário Especial</h2>

            <p class=""section-title"">CONTRATANTE</p>
            <p>
                <span class=""highlight"">Antonio Marques Ferreira Neto</span>, portador(a) do CPF: <span class=""highlight"">928.074.456-91</span>, 
                RG: <span class=""highlight"">2595594</span>, filho(a) da Sra. Stella Alice Brito, residente e domiciliado no imóvel 
                Nº 05 (Praça Roque Fiori), próximo ao complemento residencial (Amparo), no bairro Centro, da cidade de 
                São João da Boa Vista, Estado de São Paulo. Neste ato, qualificado(a) como Contratante, sendo os meios de contato 
                <span class=""highlight"">(19) 98573-3935</span> e <span class=""highlight"">antoniomarquesfneto@gmail.com</span>.
            </p>

            <p class=""section-title"">CONTRATADA</p>
            <p>
                <span class=""highlight"">Buss Viagens LTDA</span>, pessoa jurídica de direito privado, inscrita no CNPJ sob o nº 
                <span class=""highlight"">02.116.484/0001-02</span>, com sede na cidade de Goianésia, Estado de Goiás, Brasil, 
                representada neste ato pelo sócio fundador Manoel Hamilton Rodrigues, doravante denominada Contratada.
            </p>

            <p class=""section-title"">CLÁUSULA PRIMEIRA - OBJETO</p>
            <p>
                O presente contrato tem como objeto a prestação de serviço de transporte rodoviário especial, conforme 
                rota definida no registro contratual. O serviço será prestado de acordo com as especificações e condições 
                estabelecidas nas cláusulas seguintes.
            </p>

            <p class=""section-title"">CLÁUSULA SEGUNDA - VEÍCULOS</p>
            <p>
                O(s) veículo(s) a ser(em) utilizado(s) no transporte será(ão) descrito(s) a seguir: Veículo Marcopolo, 
                modelo Paradiso 1200, placa <span class=""highlight"">LYE-5316</span>, chassi <span class=""highlight"">3AF76644561AE345G</span>, 
                cor preta, ano de fabricação 2015, com capacidade para 55 passageiros. Em caso de indisponibilidade do 
                veículo designado, a Contratada poderá utilizar outro veículo habilitado no Sistema de Habilitação de 
                Transportes de Passageiros – SisHAB, da ANTT.
            </p>

            <p class=""section-title"">CLÁUSULA TERCEIRA - OBRIGAÇÕES DO CONTRATANTE</p>
            <p>
                O Contratante se compromete a cumprir rigorosamente as datas de pagamento estipuladas neste contrato, 
                sob pena de aplicação de juros moratórios de <span class=""highlight"">2% ao mês</span> sobre as parcelas em atraso.
            </p>

            <p class=""section-title"">CLÁUSULA QUARTA - PAGAMENTOS</p>
            <p>
                Os serviços serão remunerados pela Contratante à Contratada no valor total de <span class=""highlight"">R$ 566,67</span>, 
                pagos em 12 parcelas mensais de <span class=""highlight"">R$ 47,22</span>. O vencimento ocorrerá no dia 10 de cada mês, 
                sendo que a primeira parcela deverá ser quitada em até três dias úteis após a assinatura deste contrato.
            </p>

            <p class=""section-title"">CLÁUSULA QUINTA - RESCISÃO</p>
            <p>
                Em caso de rescisão antecipada deste contrato sem o devido pagamento das parcelas, será aplicada uma 
                multa de <span class=""highlight"">3%</span> sobre o valor total do contrato por cliente inadimplente, calculada sobre 
                o valor de <span class=""highlight"">R$ 566,67</span>.
            </p>

            <p class=""section-title"">CLÁUSULA SEXTA - VIGÊNCIA</p>
            <p>
                O presente contrato vigorará pelo período de <span class=""highlight"">10/04/2024</span> a <span class=""highlight"">10/04/2025</span>, 
                conforme acordado entre as partes.
            </p>

            <p class=""section-title"">CLÁUSULA SÉTIMA - DISPOSIÇÕES GERAIS</p>
            <p>
                O Contratante declara estar ciente de que somente será permitido o transporte de passageiros na 
                quantidade limitada à capacidade de assentos do(s) veículo(s) utilizado(s), sendo expressamente 
                proibido o transporte de passageiros em pé ou nos corredores. Todos os passageiros devem constar 
                na lista autorizada pela ANTT.
            </p>

            <div class=""signature-section"">
                <div class=""signature-box"">
                    <div class=""signature-line""></div>
                    <p>Assinatura do Representante Legal do Contratante</p>
                </div>
                <div class=""signature-box"">
                    <div class=""signature-line""></div>
                    <p>Assinatura do Administrador que Aprovou o Contrato</p>
                </div>
                <div class=""signature-box"">
                    <div class=""signature-line""></div>
                    <p>Assinatura da Empresa Representante</p>
                </div>
            </div>

            <div class=""footer"">
                <p>&copy; 2024 Buss Viagens LTDA. Todos os direitos reservados.</p>
                <p>Gerado em 28 de Junho de 2024.</p>
                    </div>
                </div>
            </body>
            </html>";
    }

}
