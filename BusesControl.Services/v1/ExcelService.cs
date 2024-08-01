using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Responses;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Services.v1.Interfaces;
using ClosedXML.Excel;
using Humanizer;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class ExcelService(
    INotificationApi _notificationApi,
    IContractRepository _contractRepository,
    IFinancialRepository _financialRepository,
    IInvoiceRepository _invoiceRepository,
    IInvoiceExpenseRepository _invoiceExpenseRepository
) : IExcelService
{
    public async Task<FileResponse> GenerateFinancialAsync()
    {
        try
        {
            var financialRecords = await _financialRepository.GetAllAsync();
            if (!financialRecords.Any())
            {
                _notificationApi.SetNotification(
                    statusCode: StatusCodes.Status404NotFound,
                    title: NotificationTitle.NotFound,
                    details: Message.Financial.NotFound
                );
                return default!;
            }

            using var sheetBook = new XLWorkbook();
            var sheet = sheetBook.Worksheets.Add("Sample Sheet");

            sheet.Cell(1, "A").Value = "Referência";
            sheet.Cell(1, "B").Value = "Tipo";
            sheet.Cell(1, "C").Value = "Credor/Devedor";
            sheet.Cell(1, "D").Value = "Valor Total";
            sheet.Cell(1, "E").Value = "Valor Pago";
            sheet.Cell(1, "F").Value = "Tipo de pagamento";
            sheet.Cell(1, "G").Value = "Data de vencimento";
            sheet.Cell(1, "H").Value = "Status";

            var columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };

            foreach (var column in columns)
            {
                var col = sheet.Column(column);
                col.Width = 20;
                col.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                var title = sheet.Cell(1, column);
                title.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                title.Style.Font.SetBold();
                title.Style.Font.FontColor = XLColor.DarkBlue;
            }

            var columnOne = sheet.Column("A");
            var columnTwo = sheet.Column("C");
            columnOne.Width = 25;
            columnTwo.Width = 40;

            var index = 2;
            foreach (var financial in financialRecords.ToList())
            {
                var name = financial.Customer?.Name ?? financial.Supplier!.Name;
                var totPayment = financial.Type == FinancialTypeEnum.Revenue ? financial.Invoices.Where(x => x.Status == InvoiceStatusEnum.Paid).Sum(x => x.TotalPrice) : financial.InvoiceExpenses.Where(x => x.Status == InvoiceExpenseStatusEnum.Paid).Sum(x => x.TotalPrice);

                sheet.Cell(index, "A").Value = financial.Reference;
                sheet.Cell(index, "B").Value = financial.Type.Humanize();
                sheet.Cell(index, "C").Value = name;
                sheet.Cell(index, "D").Value = financial.TotalPrice.ToString("C2");
                sheet.Cell(index, "E").Value = totPayment.ToString("C2");
                sheet.Cell(index, "F").Value = financial.PaymentType.Humanize();
                sheet.Cell(index, "G").Value = financial.TerminateDate.ToString("dd/MM/yyyy");
                sheet.Cell(index, "H").Value = financial.Active ? "Ativa" : "Inativa";

                index++;
            }

            using MemoryStream stream = new();
            sheetBook.SaveAs(stream);

            return new FileResponse
            {
                FileContent = stream.ToArray(),
                FileName = "Buses Control - Financeiro.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }
        catch (Exception ex)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: ex.Message
            );
            return default!;
        }
    }

    public async Task<FileResponse> GenerateContractAsync()
    {
        try
        {
            var contractRecords = await _contractRepository.GetAllAsync();
            if (!contractRecords.Any())
            {
                _notificationApi.SetNotification(
                    statusCode: StatusCodes.Status404NotFound,
                    title: NotificationTitle.NotFound,
                    details: Message.Contract.NotFound
                );
                return default!;
            }

            var sheetBook = new XLWorkbook();
            var sheet = sheetBook.Worksheets.Add("Sample sheet");

            sheet.Cell(1, "A").Value = "Referência";
            sheet.Cell(1, "B").Value = "Situação";
            sheet.Cell(1, "C").Value = "Aprovação";
            sheet.Cell(1, "D").Value = "Aprovador";
            sheet.Cell(1, "E").Value = "Data de início";
            sheet.Cell(1, "F").Value = "Data de término";
            sheet.Cell(1, "G").Value = "Motorista Titular";
            sheet.Cell(1, "H").Value = "Ônibus Titular";
            sheet.Cell(1, "I").Value = "Tipo de pagamento";
            sheet.Cell(1, "J").Value = "Valor Total";
            sheet.Cell(1, "K").Value = "Clientes vinculados";

            var columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };

            foreach (var column in columns)
            {
                var col = sheet.Column(column);

                col.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                col.Width = 25;

                var title = sheet.Cell(1, column);
                title.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                title.Style.Font.SetBold();
                title.Style.Font.FontColor = XLColor.DarkBlue;
            }

            var colApprover = sheet.Column("D");
            var colDriver = sheet.Column("G");
            colApprover.Width = 35;
            colDriver.Width = 35;

            var index = 2;
            foreach (var contract in contractRecords)
            {
                sheet.Cell(index, "A").Value = contract.Reference;
                sheet.Cell(index, "B").Value = contract.Status.Humanize();
                sheet.Cell(index, "C").Value = contract.IsApproved ? "Aprovado" : "Não aprovado";
                sheet.Cell(index, "D").Value = contract.Approver?.Name ?? "Não possui";
                sheet.Cell(index, "E").Value = contract.StartDate is not null ? contract.StartDate.Value.ToString("dd/MM/yyyy") : "Não possui";
                sheet.Cell(index, "F").Value = contract.TerminateDate.ToString("dd/MM/yyyy");
                sheet.Cell(index, "G").Value = contract.Driver.Name;
                sheet.Cell(index, "H").Value = $"{contract.Bus.Name} - {contract.Bus.LicensePlate}";
                sheet.Cell(index, "I").Value = contract.PaymentType.Humanize();
                sheet.Cell(index, "J").Value = contract.TotalPrice.ToString("C2");
                sheet.Cell(index, "K").Value = $"{contract.CustomersCount}";

                index++;
            }

            using MemoryStream stream = new();
            sheetBook.SaveAs(stream);

            return new FileResponse
            {
                FileContent = stream.ToArray(),
                FileName = "Buses Control - Contratos.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }
        catch (Exception ex)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: ex.Message
            );
            return default!;
        }
    }

    public async Task<FileResponse> GenerateInvoiceByFinancialAsync(Guid financialId)
    {
        try
        {
            var invoiceRecords = await _invoiceRepository.FindByFinancialAsync(financialId);
            if (!invoiceRecords.Any())
            {
                _notificationApi.SetNotification(
                    statusCode: StatusCodes.Status404NotFound,
                    title: NotificationTitle.NotFound,
                    details: Message.Invoice.NotFound
                );
                return default!;
            }

            var sheetBook = new XLWorkbook();
            var sheet = sheetBook.Worksheets.Add("Sample sheet");

            sheet.Cell(1, "A").Value = "Referência";
            sheet.Cell(1, "B").Value = "Título";
            sheet.Cell(1, "C").Value = "Situação";
            sheet.Cell(1, "D").Value = "Método de pagamento";
            sheet.Cell(1, "E").Value = "Data de vencimento";
            sheet.Cell(1, "F").Value = "Juros";
            sheet.Cell(1, "G").Value = "Valor";
            sheet.Cell(1, "H").Value = "Valor Total";

            var columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };

            foreach (var column in columns)
            {
                var col = sheet.Column(column);

                col.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                col.Width = 25;

                var title = sheet.Cell(1, column);
                title.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                title.Style.Font.SetBold();
                title.Style.Font.FontColor = XLColor.DarkBlue;
            }

            var index = 2;
            foreach (var invoice in invoiceRecords)
            {
                sheet.Cell(index, "A").Value = invoice.Reference;
                sheet.Cell(index, "B").Value = invoice.Title;
                sheet.Cell(index, "C").Value = invoice.Status.Humanize();
                sheet.Cell(index, "D").Value = invoice.PaymentMethod is not null ? invoice.PaymentMethod.Humanize() : "Não possui";
                sheet.Cell(index, "E").Value = invoice.DueDate.ToString("dd/MM/yyyy");
                sheet.Cell(index, "F").Value = invoice.InterestRate.ToString("C2");
                sheet.Cell(index, "G").Value = invoice.Price.ToString("C2");
                sheet.Cell(index, "H").Value = invoice.TotalPrice.ToString("C2");

                index++;
            }

            using MemoryStream stream = new();
            sheetBook.SaveAs(stream);

            return new FileResponse
            {
                FileContent = stream.ToArray(),
                FileName = "Buses Control - Faturas.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }
        catch (Exception ex)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: ex.Message
            );
            return default!;
        }
    }

    public async Task<FileResponse> GenerateInvoiceExpenseByFinancialAsync(Guid financialId)
    {
        try
        {
            var invoiceExpenseRecords = await _invoiceExpenseRepository.FindByFinancialAsync(financialId);
            if (!invoiceExpenseRecords.Any())
            {
                _notificationApi.SetNotification(
                    statusCode: StatusCodes.Status404NotFound,
                    title: NotificationTitle.NotFound,
                    details: Message.InvoiceExpense.NotFound
                );
                return default!;
            }

            var sheetBook = new XLWorkbook();
            var sheet = sheetBook.Worksheets.Add("Sample sheet");

            sheet.Cell(1, "A").Value = "Referência";
            sheet.Cell(1, "B").Value = "Título";
            sheet.Cell(1, "C").Value = "Situação";
            sheet.Cell(1, "D").Value = "Método de pagamento";
            sheet.Cell(1, "E").Value = "Data de vencimento";
            sheet.Cell(1, "F").Value = "Juros";
            sheet.Cell(1, "G").Value = "Valor";
            sheet.Cell(1, "H").Value = "Valor Total";

            var columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };

            foreach (var column in columns)
            {
                var col = sheet.Column(column);

                col.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                col.Width = 25;

                var title = sheet.Cell(1, column);
                title.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                title.Style.Font.SetBold();
                title.Style.Font.FontColor = XLColor.DarkBlue;
            }

            var index = 2;
            foreach (var invoice in invoiceExpenseRecords)
            {
                sheet.Cell(index, "A").Value = invoice.Reference;
                sheet.Cell(index, "B").Value = invoice.Title;
                sheet.Cell(index, "C").Value = invoice.Status.Humanize();
                sheet.Cell(index, "D").Value = invoice.PaymentMethod is not null ? invoice.PaymentMethod.Humanize() : "Não possui";
                sheet.Cell(index, "E").Value = invoice.DueDate.ToString("dd/MM/yyyy");
                sheet.Cell(index, "F").Value = invoice.InterestRate.ToString("C2");
                sheet.Cell(index, "G").Value = invoice.Price.ToString("C2");
                sheet.Cell(index, "H").Value = invoice.TotalPrice.ToString("C2");

                index++;
            }

            using MemoryStream stream = new();
            sheetBook.SaveAs(stream);

            return new FileResponse
            {
                FileContent = stream.ToArray(),
                FileName = "Buses Control - Faturas de despesas.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }
        catch (Exception ex)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: ex.Message
            );
            return default!;
        }
    }
}
