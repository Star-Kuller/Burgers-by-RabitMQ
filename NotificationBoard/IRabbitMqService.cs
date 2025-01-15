using RabbitMQ.Client.Events;

namespace NotificationBoard;

public interface IRabbitMqService
{
    AsyncEventingBasicConsumer NotificationConsumer { get; }
    Task InitializeRabbitMqAsync();
}