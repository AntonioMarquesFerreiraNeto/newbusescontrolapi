using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BusesControl.Services.v1;

public class ExportService : IExportService
{
    public ExportService(
        IUnitOfWork unitOfWork, IExportRepository exportRepository, 
        IRabbitMqService rabbitMqService, INotificationContext notificationContext,
        IStorageRepository storageRepository, IOptions<AppSettings> options
    )
    {
        _unitOfWork = unitOfWork;
        _exportRepository = exportRepository;
        _rabbitMqService = rabbitMqService;
        _notificationContext = notificationContext;
        _storageRepository = storageRepository;
        _settings = options.Value;
    }

    private readonly IExportRepository _exportRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly INotificationContext _notificationContext;
    private readonly IStorageRepository _storageRepository;
    private readonly AppSettings _settings;

    public async Task<PaginationResponse<ExportModel>> GetPaginatedAsync(PaginationRequest request)
    {
        return await _exportRepository.GetPaginatedAsync(request.Page, request.PageSize);
    }

    public async Task<FileResponse> GetFileAsync(string fileName)
    {
        var fileResponse = await _storageRepository.GetFileAsync(_settings.Azure.Storage.ContainerName, fileName);
        if (fileResponse is null)
        {
            _notificationContext.SetNotification(
                title: NotificationTitle.NotFound,
                details: Message.Commons.ResourceNotFound,
                statusCode: StatusCodes.Status404NotFound
            );

            return default!;
        }

        return fileResponse;
    }

    public async Task<bool> CreateAsync(ExportCreateRequest request)
    {
        _unitOfWork.BeginTransaction();

        var record = new ExportModel
        {
            DocumentType = request.DocumentType,
            Type = request.Type,
            Status = ExportStatusEnum.Pending,
            CreatedAt = DateTime.UtcNow,
        };

        await _exportRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        var published = await _rabbitMqService.PublishAsync(RabbitMqKey.ExportQueue, record);
        if (!published)
        {
            _unitOfWork.Rollback();

            _notificationContext.SetNotification(
                title: NotificationTitle.BadRequest,
                details: Message.Export.FailedPublished,
                statusCode: StatusCodes.Status400BadRequest
            );

            return false;
        }

        await _unitOfWork.CommitAsync(true);

        return true;
    }

    public async Task<bool> RemoveExpireds()
    {
        var records = await _exportRepository.GetExpiredsAsync();

        foreach (var item in records)
        {
            if (item.Url != null)
            {
                await _storageRepository.RemoveAsync(
                    containerName: _settings.Azure.Storage.ContainerName, 
                    fileName: item.Url
                );
            }
        }

        _exportRepository.RemoveRange(records);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
