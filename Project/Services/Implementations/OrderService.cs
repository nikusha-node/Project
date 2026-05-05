using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly List<Order> _orders = new();
    private readonly ICartService _cartService;

    public OrderService(ICartService cartService)
    {
        _cartService = cartService;
    }

    public Order Checkout(int userId)
    {
        var cart = _cartService.GetCart();

        if (!cart.Items.Any())
            return null;

        var order = new Order
        {
            Id = _orders.Count + 1,
            UserId = userId,
            Items = cart.Items.Select(i => new OrderItem
            {
                GameId = i.GameId,
                Quantity = i.Quantity,
                PriceAtPurchase = i.PriceAtPurchase
            }).ToList()
        };

        _orders.Add(order);
        _cartService.Clear();

        return order;
    }

    public List<Order> GetUserOrders(int userId)
    {
        return _orders.Where(o => o.UserId == userId).ToList();
    }
}