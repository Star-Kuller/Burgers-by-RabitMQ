using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Kitchen;

public class RabbitMqService(IChannel channel) : IRabbitMqService
{
    public AsyncEventingBasicConsumer Consumer { get; init; } = new(channel);

    public async Task InitializeRabbitMqAsync()
    {
        await channel.QueueDeclareAsync(
            "Notification",
            durable: true,
            exclusive: false,
            autoDelete: false);

        await channel.QueueDeclareAsync(
            "Kitchen",
            durable: true,
            exclusive: false,
            autoDelete: false);
        
        await channel.BasicConsumeAsync("Kitchen", autoAck: true, consumer: Consumer);
    }

    public async Task SendFinishedOrderAsync(FinishedOrder order)
    {
        var serializedOrder = JsonConvert.SerializeObject(order);
        var body = Encoding.UTF8.GetBytes(serializedOrder);
        
        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: "Notification",
            body: body);
    }
}