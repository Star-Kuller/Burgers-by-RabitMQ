using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Kitchen;

public class RabbitMqService(IChannel channel) : IRabbitMqService
{
    public AsyncEventingBasicConsumer KitchenConsumer { get; init; } = new(channel);

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
        
        await channel.BasicConsumeAsync("Kitchen", autoAck: true, consumer: KitchenConsumer);
    }

    public async Task SendFinishedOrderAsync(FinishedOrder order)
    {
        var serializedOrder = JsonConvert.SerializeObject(order);
        var body = Encoding.UTF8.GetBytes(serializedOrder);
        
        var basicProperties = new BasicProperties
        {
            Headers = new Dictionary<string, object?>()
            {
                {"message_type", nameof(FinishedOrder)}
            }
        };

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: "Notification",
            mandatory: false,
            basicProperties: basicProperties,
            body: body);
    }
}