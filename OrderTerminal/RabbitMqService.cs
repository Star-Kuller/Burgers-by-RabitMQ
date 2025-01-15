using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace OrderTerminal;

public class RabbitMqService(IChannel channel) : IRabbitMqService
{
    public async Task InitializeRabbitMqAsync()
    {
        await channel.ExchangeDeclareAsync(
            "orders_exchange",
            ExchangeType.Fanout,
            durable: true);

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

        await channel.QueueBindAsync("Notification", "orders_exchange", "");
        await channel.QueueBindAsync("Kitchen", "orders_exchange", "");
    }
    
    public async Task SendOrderAsync(Order order)
    {
        var json = JsonConvert.SerializeObject(order);
        var body = Encoding.UTF8.GetBytes(json);
        
        var basicProperties = new BasicProperties
        {
            Headers = new Dictionary<string, object?>()
            {
                {"message_type", nameof(Order)}
            }
        };

        await channel.BasicPublishAsync(
            exchange: "orders_exchange",
            routingKey: "",
            mandatory: false,
            basicProperties: basicProperties,
            body: body);
    }
}