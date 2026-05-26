using BusesControl.Entities.DTOs;
using BusesControl.Services.v1.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BusesControl.Services.v1;

public class RabbitMqService : IRabbitMqService, IAsyncDisposable
{
    public RabbitMqService(IOptions<AppSettings> options, ILogger<RabbitMqService> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    private readonly AppSettings _settings;
    private readonly ILogger _logger;
    private IConnection? _connection;

    private async Task EnsureConnectionAsync()
    {
        if (_connection is { IsOpen: true })
            return;

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.RabbitMq.HostName,
                UserName = _settings.RabbitMq.UserName,
                Password = _settings.RabbitMq.Password,
            };

            _connection = await factory.CreateConnectionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogCritical("não foi possível conectar ao RabbitMQ: {0}", ex.Message);
        }
    }

    public async Task<bool> PublishAsync<T>(string queue, T message)
    {
        try
        {            
            await EnsureConnectionAsync();

            _logger.LogInformation("iniciando publicação da mensagem, request: {@Request}", message);

            if (_connection is null)
            {
                _logger.LogError("falha ao tentar publicar devido a conexão igual a nula.");
                return false;
            }

            using var channel = await _connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);

            var messageContent = new RabbitMqMessageDto<T>
            {
                Content = message,
                PublishedAt = DateTime.UtcNow,
            };

            var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageContent));

            var specs = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: string.Empty, 
                routingKey: queue, 
                basicProperties: specs, 
                body: payload,
                mandatory: false
            );

            _logger.LogInformation($"mensagem publicada com sucesso, queue: {queue}");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("falha ao publicar mensagem, detalhes do erro: {0}", ex);
            return false;
        } 
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            _logger.LogInformation("limpando recursos do RabbitMQ...");

            if (_connection is { IsOpen: true })
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "erro ao descartar recursos do RabbitMQ");
        }
    }
}
