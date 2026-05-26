namespace BusesControl.Entities.DTOs
{
    public class RabbitMqMessageDto<T>
    {
        public T Content { get; set; } = default!;
        public DateTime PublishedAt { get; set; } = default;
    }
}
