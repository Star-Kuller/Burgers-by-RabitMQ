namespace NotificationBoard;

public class FinishedOrder
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public List<string> Dishes { get; set; }
    public TimeSpan CookingTime { get; set; }
}