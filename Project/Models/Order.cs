using System.Collections.Generic;

namespace Project.Models;

public class Order : BaseEntity
{
    
    public int UserId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
