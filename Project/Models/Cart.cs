using System.Collections.Generic;
using System.Linq;

namespace Project.Models;

public class Cart : BaseEntity
{
    public List<OrderItem> Items { get; set; } = new();

    public decimal TotalPrice
    {
        get
        {
            return Items.Sum(i => i.PriceAtPurchase * i.Quantity);
        }
    }

    public void Clear()
    {
        Items.Clear();
    }
}
