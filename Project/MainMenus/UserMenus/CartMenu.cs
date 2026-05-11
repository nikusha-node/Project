using Project.Helpers;
using Project.Services.Interfaces;

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
            Console.Clear();
            LogoHelper.ShowLogo();

            UIHelper.Divider();
            UIHelper.WriteLineCentered("🧾  MY CART  🧾", ConsoleColor.Yellow);
            UIHelper.Divider();

            var cart = _cartService.GetCart();

            if (!cart.Items.Any())
            {
                UIHelper.Warning("Your cart is empty!");
            }
            else
            {
                UIHelper.WriteLineCentered(
                    $"{"GAME ID",-10} {"QUANTITY",-10} {"PRICE",-10}",
                    ConsoleColor.Magenta
                );
                UIHelper.WriteLineCentered(new string('─', 35), ConsoleColor.DarkGray);

                foreach (var item in cart.Items)
                {
                    UIHelper.WriteLineCentered(
                        $"{item.GameId,-10} {item.Quantity,-10}",
                        ConsoleColor.White
                    );
                }

                UIHelper.Divider();
                UIHelper.WriteLineCentered(
                    $"💵  Total: {cart.TotalPrice}$",
                    ConsoleColor.Green
                );
            }

            UIHelper.Divider();
            UIHelper.WriteLineCentered("1.  ❌  Remove Item", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("2.  ✅  Checkout", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("0.  🚪  Back", ConsoleColor.DarkGray);
            UIHelper.Divider();
            UIHelper.WriteLineCentered("Enter your choice:", ConsoleColor.White);

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    UIHelper.Info("Enter Game ID to remove:");
                    int id = int.TryParse(Console.ReadLine(), out var result) ? result : 0;
                    _cartService.RemoveFromCart(id);
                    UIHelper.Success("Item removed!");
                    Console.ReadKey();
                    break;

                case "2":
                    var user = _authService.GetCurrentUser();
                    if (user == null)
                    {
                        UIHelper.Error("You must be logged in to checkout!");
                        Console.ReadKey();
                        break;
                    }
                    var order = _orderService.Checkout(user.Id);
                    if (order == null)
                        UIHelper.Warning("Cart is empty, nothing to checkout!");
                    else
                        UIHelper.Success($"🎉 Order #{order.Id} placed successfully!");
                    Console.ReadKey();
                    break;

                case "0":
                    return;

                default:
                    UIHelper.Error("Invalid choice!");
                    Console.ReadKey();
                    break;
            }
        }
    }
}