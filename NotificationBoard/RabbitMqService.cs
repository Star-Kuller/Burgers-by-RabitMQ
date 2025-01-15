using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationBoard;

public class RabbitMqService(IChannel channel) : IRabbitMqService
{
    public AsyncEventingBasicConsumer NotificationConsumer { get; init; } = new(channel);

    public async Task InitializeRabbitMqAsync()
    {
        await channel.QueueDeclareAsync(
            "Notification",
            durable: true,
            exclusive: false,
            autoDelete: false);
        
        await channel.BasicConsumeAsync("Notification", autoAck: true, consumer: NotificationConsumer);
    }
}