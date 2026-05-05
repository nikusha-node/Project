using Project.Services.Interfaces;
using System.Linq;

namespace Project.UserMenus;

public class CartMenu 
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly IAuthService _authService;

    public CartMenu(ICartService cartService, IOrderService orderService, IAuthService authService)
    {
        _cartService = cartService;
        _orderService = orderService;
        _authService = authService;
    }

    public void Show()
    {
        while (true)
        {
            var cart = _cartService.GetCart();

            Console.WriteLine("\n=== CART ===");

            if (!cart.Items.Any())
            {
                Console.WriteLine("Cart is empty");
            }
            else
            {
                foreach (var item in cart.Items)
                {
                    Console.WriteLine($"GameId: {item.GameId} | Qty: {item.Quantity}");
                }

                Console.WriteLine($"Total: {cart.TotalPrice}$");
            }

            Console.WriteLine("1. Remove Item");
            Console.WriteLine("2. Checkout");
            Console.WriteLine("0. Back");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Game Id: ");
                    int id = int.TryParse(Console.ReadLine(), out var result) ? result : 0;
                    _cartService.RemoveFromCart(id);
                    break;

                case "2":
                    var user = _authService.GetCurrentUser();

                    if (user == null)
                    {
                        Console.WriteLine("You must login!");
                        break;
                    }

                    var order = _orderService.Checkout(user.Id);

                    if (order == null)
                        Console.WriteLine("Cart is empty!");
                    else
                        Console.WriteLine("Order placed!");

                    break;

                case "0":
                    return;
            }
        }
    }
}