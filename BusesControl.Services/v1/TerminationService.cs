using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Response;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1;

public class TerminationService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    ITerminationRepository _terminationRepository,
    ITerminationBusiness _terminationBusiness
) : ITerminationService
{
    public async Task<SuccessResponse> CreateAsync(Guid contractId, TerminationCreateRequest request)
    {
        var contractRecord = await _terminationBusiness.GetForCreateAsync(contractId, request.CustomerId);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        //TODO: atualizar active do financial para false e cancelar todas as faturas do cliente no assas e na nossa base.
        //TODO: criar uma financial para o processo de rescisão. 
        var price = Math.Round(contractRecord.TotalPrice * contractRecord.SettingPanel.TerminationFee / 100, 2);

        _unitOfWork.BeginTransaction();

        var financialId = Guid.NewGuid();

        var record = new TerminationModel 
        { 
            ContractId = contractId,
            CustomerId = request.CustomerId,
            FinancialId = financialId,
            Price = price
        };
        await _terminationRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.Termination.CreateOk);
    }
}
