using BusesControl.Services.v1.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BusesControl.Services.v1.Background;

//TODO: analisar a possibilidade de usar Azure Logic Apps.
//TODO: implementar feature flag no banco de dados para controlar se irei rodar as rotinas ou não, ou desabilitar algumas rotinas em específico.
public class BackgroundTaskRoutineService : BackgroundService
{
    public BackgroundTaskRoutineService(ILogger<BackgroundTaskRoutineService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    private readonly int _delay = 5;
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) 
        {
            try
            {
                var dateNow = DateTime.Now;
                
                var nextToday = DateTime.Today.AddDays(1);
                var difference = nextToday - dateNow;

                await Task.Delay(difference, stoppingToken);

                _logger.LogInformation("processing started process in {0}", DateTime.Now);

                await AutomatedPaymentAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(_delay), stoppingToken);
                await AutomatedOverdueInvoiceProcessingAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(_delay), stoppingToken);
                await AutomatedContractFinalizationAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(_delay), stoppingToken);
                await AutomatedCancelProcessTerminationAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(_delay), stoppingToken);
                await AutomatedChangeWebhookAsync(stoppingToken);

                _logger.LogInformation("Finalization process in {0}", DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error in background task routine service. Error: : {0}", ex);
            }
        }
    }

    private async Task AutomatedChangeWebhookAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Automated Change Webhook started at {Time}", DateTime.Now);

        using var scope =  _serviceScopeFactory.CreateScope();
        var systemService = scope.ServiceProvider.GetRequiredService<ISystemService>();
        await systemService.AutomatedChangeWebhookAsync();

        _logger.LogInformation("Automated Change Webhook finished at {Time}", DateTime.Now);
    }

    private async Task AutomatedPaymentAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Automated Payment started at {Time}", DateTime.Now);

        using var scope = _serviceScopeFactory.CreateScope();
        var systemService = scope.ServiceProvider.GetRequiredService<ISystemService>();
        await systemService.AutomatedPaymentAsync();

        _logger.LogInformation("Automated Payment finished at {Time}", DateTime.Now);
    }

    private async Task AutomatedOverdueInvoiceProcessingAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Automated Overdue Invoice Processing started at {Time}", DateTime.Now);

        using var scope = _serviceScopeFactory.CreateScope();
        var systemService = scope.ServiceProvider.GetRequiredService<ISystemService>();
        await systemService.AutomatedOverdueInvoiceProcessingAsync();

        _logger.LogInformation("Automated Overdue Invoice Processing finished at {Time}", DateTime.Now);
    }

    private async Task AutomatedContractFinalizationAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Automated Contract Finalization started at {Time}", DateTime.Now);

        using var scope = _serviceScopeFactory.CreateScope();
        var systemService = scope.ServiceProvider.GetRequiredService<ISystemService>();
        await systemService.AutomatedContractFinalizationAsync();

        _logger.LogInformation("Automated Contract Finalization finished at {Time}", DateTime.Now);
    }

    private async Task AutomatedCancelProcessTerminationAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Automated Cancel Process Termination started at {Time}", DateTime.Now);

        using var scope = _serviceScopeFactory.CreateScope();
        var systemService = scope.ServiceProvider.GetRequiredService<ISystemService>();
        await systemService.AutomatedCancelProcessTerminationAsync();

        _logger.LogInformation("Automated Cancel Process Termination finished at {Time}", DateTime.Now);
    }
}
