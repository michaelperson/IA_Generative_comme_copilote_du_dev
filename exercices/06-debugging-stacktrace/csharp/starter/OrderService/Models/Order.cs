namespace OrderService.Models;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
