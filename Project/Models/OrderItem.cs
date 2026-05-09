namespace Project.Models;

public class OrderItem : BaseEntity
{
    public int GameId { get; set; }

    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }
}
