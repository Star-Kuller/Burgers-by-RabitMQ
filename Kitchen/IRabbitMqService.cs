using RabbitMQ.Client.Events;

namespace Kitchen;

public interface IRabbitMqService
{
    AsyncEventingBasicConsumer KitchenConsumer { get; }
    Task InitializeRabbitMqAsync();
    Task SendFinishedOrderAsync(FinishedOrder order);
}