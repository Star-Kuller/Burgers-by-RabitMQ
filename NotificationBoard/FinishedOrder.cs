namespace NotificationBoard;

public class FinishedOrder(Order order)
{
    public Guid Id { get; set; } = order.Id;
    public int OrderNumber { get; set; } = order.OrderNumber;
    public List<string> Dishes { get; set; } = order.Dishes.Select(x => x.GetDishName()).ToList();
    public TimeSpan CookingTime { get; set; }
}