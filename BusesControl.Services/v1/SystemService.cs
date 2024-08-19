using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BusesControl.Services.v1;

public class SystemService(
    AppSettings _appSettings,
    IUnitOfWork _unitOfWork,
    UserManager<UserModel> _userManager,
    IUserService _userService,
    IContractService _contractService,
    IInvoiceService _invoiceService,
    INotificationService _notificationService,
    IWebhookService _webhookService,
    ICustomerContractService _customerContractService,
    IContractRepository _contractRepository,
    ICustomerContractRepository _customerContractRepository,
    ISavedCardRepository _savedCardRepository,
    IInvoiceRepository _invoiceRepository,
    IWebhookRepository _webhookRepository
) : ISystemService
{
    public async Task<SystemResponse> AutomatedChangePasswordUserSystem()
    {
        var systemResponse = new SystemResponse();

        var users = await _userManager.GetUsersInRoleAsync(_appSettings.UserSystem.Role);
        if (users.Count == 0)
        {
            systemResponse.FailureOperation.Add("Nenhum usuário de sistema encontrado.");
            return systemResponse;
        }

        var userSystem = users.First();
        var newPassword = _userService.GeneratePassword();
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userSystem);

        var userResult = await _userManager.ResetPasswordAsync(userSystem, resetToken, newPassword);
        if (!userResult.Succeeded)
        {
            systemResponse.FailureOperation.Add("Desculpe, houve uma falha na operação.");
            return systemResponse;
        }

        systemResponse.SuccessOperation.Add("Senha de usuário de sistema atualizada com sucesso.");

        return systemResponse;
    }

    public async Task<SystemResponse> AutomatedChangeWebhookAsync()
    {
        var systemResponse = new SystemResponse();

        var webhookRecords = await _webhookRepository.GetAllAsync();
        if (!webhookRecords.Any())
        {
            systemResponse.NoOperation = Message.Commons.NoOperation;
        }

        foreach (var webhook in webhookRecords)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var webhookChangeTokenResponse = await _webhookService.ChangeInternalAsync(webhook);
                if (!webhookChangeTokenResponse.Success)
                {
                    systemResponse.FailureOperation.Add($"Webhook ({webhook.Name}). \nDetalhes do erro: {webhookChangeTokenResponse.ErrorMessage}");
                    _unitOfWork.Rollback();
                    continue;
                }

                await _unitOfWork.CommitAsync(true);

                systemResponse.SuccessOperation.Add($"Webhook ({webhook.Name}) atualizado com sucesso");
            } 
            catch (Exception ex) 
            {
                systemResponse.FailureOperation.Add($"Falha ao tentar atualizar o token do webhook - {webhook.Name}. \nDetalhes do erro: {ex.Message}");
                _unitOfWork.Rollback();
            }
        }

        return systemResponse;
    }

    public async Task<SystemResponse> AutomatedPaymentAsync(DateTime? date = null)
    {
        var systemResponse = new SystemResponse();
        
        date ??= DateTime.UtcNow;

        var invoiceRecords = await _invoiceRepository.FindByDueDateForSystemAsync(DateOnly.FromDateTime(date.Value));
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
                    systemResponse.FailureOperation.Add($"Pagamento da fatura {invoiceRecord.Reference} falhou por ausência de informações do cartão de crédito.");
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

        var invoiceRecords = await _invoiceRepository.FindByStatusForSystemWithFinancialAsync(InvoiceStatusEnum.Pending);
        if (!invoiceRecords.Any())
        {
            systemResponse.NoOperation = Message.Commons.NoOperation;
        }

        foreach (var invoiceRecord in invoiceRecords)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var (success, errorMessage) = await _invoiceService.ChangeOverDueInternalAsync(invoiceRecord);
                if (!success)
                {
                    systemResponse.FailureOperation.Add(errorMessage!);
                    _unitOfWork.Rollback();
                    continue;
                }

                await _notificationService.SendInternalNotificationAsync(TemplateTitle.InvoiceOverDue, TemplateMessage.InvoiceOverDue(invoiceRecord.Reference), NotificationAccessLevelEnum.Public);

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

    public async Task<SystemResponse> AutomatedContractFinalizationAsync()
    {
        var systemResponse = new SystemResponse();

        var contractRecords = await _contractRepository.FindByContractAndTerminateDateAsync(ContractStatusEnum.InProgress, DateOnly.FromDateTime(DateTime.UtcNow));
        if (!contractRecords.Any()) 
        {
            systemResponse.NoOperation = Message.Commons.NoOperation;
            return systemResponse;
        }

        foreach (var contract in contractRecords)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await _contractService.CompletedWithoutValidationAsync(contract);

                await _notificationService.SendInternalNotificationAsync(TemplateTitle.ContractCompleted, TemplateMessage.ContractCompleted(contract.Reference), NotificationAccessLevelEnum.Admin);

                await _unitOfWork.CommitAsync(true);

                systemResponse.SuccessOperation.Add($"Contrato {contract.Reference} concluído com sucesso.");
            } 
            catch(Exception ex)
            {
                systemResponse.FailureOperation.Add($"Falha na conclusão do contrato {contract.Reference}. Detalhes do erro: {ex.Message}");
                _unitOfWork.Rollback();
            }
        }

        return systemResponse;
    }

    public async Task<SystemResponse> AutomatedCancelProcessTerminationAsync()
    {
        var systemResponse = new SystemResponse();

        var customerContractRecords = await _customerContractRepository.FindByProcessTerminationAsync(true);

        customerContractRecords = customerContractRecords.Where(x => x.ProcessTerminationDate!.Value.Date >= DateTime.UtcNow.Date.AddDays(2));
        if (!customerContractRecords.Any())
        {
            systemResponse.NoOperation = Message.Commons.NoOperation;
            return systemResponse;
        }

        foreach (var customerContract in customerContractRecords)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await _customerContractService.ToggleProcessTerminationWithOutValidationAsync(customerContract);

                await _unitOfWork.CommitAsync(true);

                systemResponse.SuccessOperation.Add($"Sucesso no cancelamento do processo de rescisão do contrato {customerContract.ContractId} do cliente {customerContract.CustomerId}.");
            }
            catch (Exception ex)
            {
                systemResponse.FailureOperation.Add($"Falha no cancelamento do processo de rescisão do contrato {customerContract.ContractId} do cliente {customerContract.CustomerId}. Detalhes do erro: {ex.Message}");
                _unitOfWork.Rollback();
            }
        }

        return systemResponse;
    }
}
