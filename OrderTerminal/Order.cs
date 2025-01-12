namespace OrderTerminal;

public class Order (List<int> dishes)
{
    private static int _orderCounter = 0;
    public Guid Id { get; set; } = Guid.NewGuid();
    public int OrderNumber { get; set; } = _orderCounter++;
    public List<int> Dishes { get; set; } = dishes;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}