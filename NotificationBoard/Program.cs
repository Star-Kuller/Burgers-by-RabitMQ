using System.Text;
using Newtonsoft.Json;
using NotificationBoard;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "rabbitmq" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();
IRabbitMqService rabbitMqService = new RabbitMqService(channel);
var random = new Random();
rabbitMqService.Consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var serializedOrder = Encoding.UTF8.GetString(body);
    var order = JsonConvert.DeserializeObject<Order>(serializedOrder);
    if (order is null)
    {
        Console.WriteLine($"Хм... Как это сюда попало? {serializedOrder}");
        return Task.CompletedTask;
    }
    
    Console.WriteLine($"Начали готовить заказ {order.Id}");
    foreach (var dishName in order.Dishes.Select(dishId => dishId.GetDishName()))
    {
        Task.Delay(random.Next(300, 10000));
        Console.WriteLine($"Блюдо {dishName} для заказа {order.Id} готово!");
    }
    var finishedOrder = new FinishedOrder(order)
    {
        CookingTime = DateTime.UtcNow - order.CreatedAt
    };
    Console.WriteLine($"Заказ {order.Id} готов!");
    
    rabbitMqService.SendFinishedOrderAsync(finishedOrder);
    return Task.CompletedTask;
};
await rabbitMqService.InitializeRabbitMqAsync();