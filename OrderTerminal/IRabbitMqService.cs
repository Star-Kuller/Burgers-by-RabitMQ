namespace OrderTerminal;

public interface IRabbitMqService
{
    Task InitializeRabbitMqAsync();
    Task SendOrderAsync(Order order);
}