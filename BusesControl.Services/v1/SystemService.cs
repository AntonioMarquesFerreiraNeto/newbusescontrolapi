using BusesControl.Commons.Notification;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Responses;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1;

public class SystemService(
    IUnitOfWork _unitOfWork,
    IInvoiceService _invoiceService,
    ISavedCardRepository _savedCardRepository,
    IInvoiceRepository _invoiceRepository
) : ISystemService
{
    //TODO: Implementar método que recebe a notificação do assas do pagamento via PIX realizado.

    public async Task<SystemResponse> AutomatedPaymentAsync(DateTime? date = null)
    {
        var systemResponse = new SystemResponse();
        
        date ??= DateTime.UtcNow;

        var invoiceRecords = await _invoiceRepository.FindByDueDateForSystemAsync(DateOnly.FromDateTime(date.Value), true);
        if (!invoiceRecords.Any())
        {
            systemResponse.NoOperation = Message.Commons.NoOperation;
        }

        foreach (var invoiceRecord in invoiceRecords)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var savedCardRecord = await _savedCardRepository.GetByCustomerAsync(invoiceRecord.Financial.CustomerId!.Value);
                if (savedCardRecord is null)
                {
                    _unitOfWork.Rollback();
                    continue;
                }

                var result = await _invoiceService.AutomatedPaymentAsync(invoiceRecord, savedCardRecord.CreditCardToken);
                if (!result.Success)
                {
                    systemResponse.FailureOperation.Add($"Pagamento da fatura {invoiceRecord.Reference} falhou na integração. Detalhes da falha: {result.MessageFailure}");
                    _unitOfWork.Rollback();
                    continue;
                }

                await _unitOfWork.CommitAsync(true);

                systemResponse.SuccessOperation.Add($"Pagamento da fatura {invoiceRecord.Reference} realizado com sucesso");
            }
            catch (Exception ex)
            {
                systemResponse.FailureOperation.Add($"Pagamento da fatura {invoiceRecord.Reference} falhou internamente. Detalhes da falha: {ex.Message}");
                _unitOfWork.Rollback();
            }
        }

        return systemResponse;
    }

    public async Task<SystemResponse> AutomatedOverdueInvoiceProcessingAsync()
    {
        var systemResponse = new SystemResponse();

        var invoiceRecords = await _invoiceRepository.FindByStatusForSystemAsync(InvoiceStatusEnum.Pending);
        if (!invoiceRecords.Any())
        {
            systemResponse.NoOperation = Message.Commons.NoOperation;
        }

        foreach (var invoiceRecord in invoiceRecords)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var (success, errorMessage) = await _invoiceService.ChangeOverDueForSystemAsync(invoiceRecord);
                if (!success)
                {
                    systemResponse.FailureOperation.Add(errorMessage!);
                    _unitOfWork.Rollback();
                    continue;
                }

                await _unitOfWork.CommitAsync(true);

                systemResponse.SuccessOperation.Add($"Fatura {invoiceRecord.Reference} atualizada com sucesso");
            }
            catch (Exception ex)
            {
                systemResponse.FailureOperation.Add($"Atualização da fatura {invoiceRecord.Reference} falhou. Detalhes do erro: {ex.Message}");
                _unitOfWork.Rollback();
            }
        }

        return systemResponse;
    }
}
