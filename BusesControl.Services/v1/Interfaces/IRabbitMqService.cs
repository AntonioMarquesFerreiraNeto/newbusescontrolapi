namespace BusesControl.Services.v1.Interfaces;

public interface IRabbitMqService
{
    Task<bool> PublishAsync<T>(string queue, T message);
}
