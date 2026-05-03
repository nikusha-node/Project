namespace Project.Models;

public class OrderItem
{
    public int GameId { get; set; }

    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }
}
