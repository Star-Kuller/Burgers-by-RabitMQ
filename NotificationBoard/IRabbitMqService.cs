using RabbitMQ.Client.Events;

namespace NotificationBoard;

public interface IRabbitMqService
{
    AsyncEventingBasicConsumer Consumer { get; }
    Task InitializeRabbitMqAsync();
    Task SendFinishedOrderAsync(FinishedOrder order);
}