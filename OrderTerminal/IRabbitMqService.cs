namespace OrderTerminal;

public interface IRabbitMqService
{ 
    Task SendOrder(Order order);
}