using BusesControl.Entities.DTOs;

namespace BusesControl.Services.v1.Interfaces;

public interface IInvoiceService
{
    Task<bool> CreateForFinancialAsync(CreateInvoiceDTO createInvoice);
}
