using System.Text;
using Newtonsoft.Json;
using NotificationBoard;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "rabbitmq" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();
IRabbitMqService rabbitMqService = new RabbitMqService(channel);

rabbitMqService.NotificationConsumer.ReceivedAsync += async (sender, @event) =>
{
    var headers = @event.BasicProperties.Headers;
    var messageTypeHeader = headers?["message_type"];
    if(messageTypeHeader is null)
        return;
    var messageType = Encoding.UTF8.GetString((byte[])messageTypeHeader);
    
    var body = @event.Body.ToArray();
    var json = Encoding.UTF8.GetString(body);
    switch (messageType)
    {
        case nameof(Order):
        {
            var order = JsonConvert.DeserializeObject<Order>(json);
            if(order is null)
                return;
            Console.WriteLine($"Поступил заказ {order.OrderNumber}");
            return;
        }
        case nameof(FinishedOrder):
        {
            var order = JsonConvert.DeserializeObject<FinishedOrder>(json);
            if(order is null)
                return;
            Console.WriteLine($"Заказ {order.OrderNumber} готов! Приготовлен за {order.CookingTime}");
            Console.WriteLine($"Можете забирать {string.Join(", ", order.Dishes)}");
            return;
        }
    }
};
await rabbitMqService.InitializeRabbitMqAsync();
while (true) { }