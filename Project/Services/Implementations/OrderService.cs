using Project.Data;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ICartService _cartService;
        private readonly DatabaseContext _db;

        private const string PATH = "orders.json";

        public OrderService(ICartService cartService, DatabaseContext db)
        {
            _cartService = cartService;
            _db = db;

            var data = FileHandler.LoadFromFile<List<Order>>(PATH);

            if (data != null)
                _db.Orders = data;
        }

        public Order? Checkout(int userId)
        {
            var cart = _cartService.GetCart();

            if (!cart.Items.Any())
                return null;

            var order = new Order
            {
                Id = _db.Orders.Count + 1,
                UserId = userId,
                Items = new List<OrderItem>(cart.Items),
                TotalPrice = cart.TotalPrice,
                CreatedAt = DateTime.Now
            };

            _db.Orders.Add(order);

            Save();

            cart.Items.Clear();

            return order;
        }

        public List<Order> GetAll()
        {
            return _db.Orders;
        }

        private void Save()
        {
            FileHandler.SaveToFile(PATH, _db.Orders);
        }

        public List<Order> GetUserOrders(int userId)
        {
            return _db.Orders
                .Where(o => o.UserId == userId)
                .ToList();
        }
    }
}