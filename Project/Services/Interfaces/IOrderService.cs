using Project.Models;

namespace Project.Services.Interfaces;

public interface IOrderService
{
    Order Checkout(int userId);

    List<Order> GetUserOrders(int userId);
}