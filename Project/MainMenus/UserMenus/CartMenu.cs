using Project.Helpers;
using Project.Services.Implementations;
using Project.Services.Interfaces;

namespace Project.UserMenus;

public class CartMenu
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly IAuthService _authService;
    private readonly IGameService _gameService;

    public CartMenu(ICartService cartService, IOrderService orderService, IAuthService authService, IGameService gameService)
    {
        _cartService = cartService;
        _orderService = orderService;
        _authService = authService;
        _gameService = gameService;
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
                    $"{"ID",-5} {"NAME",-22} {"QUANTITY",-10} {"PRICE",-10}",
                    ConsoleColor.Magenta
                );
                UIHelper.WriteLineCentered(new string('─', 35), ConsoleColor.DarkGray);

                foreach (var item in cart.Items)
                {
                    var game = _gameService.GetById(item.GameId);
                    var gameName = game?.Name ?? "Unknown";
                    UIHelper.WriteLineCentered($"{item.GameId,-5} {gameName,-22} {item.Quantity,-10} {item.PriceAtPurchase,-10}$",
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
            UIHelper.WriteLineCentered("3.  📋  Order History", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("0.  🚪  Back", ConsoleColor.DarkGray);
            UIHelper.Divider();
            UIHelper.WriteLineCentered("Enter your choice:", ConsoleColor.White);

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    int id = InputHelper.ReadInt("Enter Game ID to remove: ");
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

                case "3":
                    ShowOrderHistory();
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
    private void ShowOrderHistory()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("📋  ORDER HISTORY  📋", ConsoleColor.Yellow);
        UIHelper.Divider();

        var user = _authService.GetCurrentUser();
        var orders = _orderService.GetUserOrders(user.Id);

        if (!orders.Any())
        {
            UIHelper.Warning("No orders yet!");
        }
        else
        {
            foreach (var order in orders)
            {
                UIHelper.WriteLineCentered(
                    $"Order #{order.Id}  |  {order.CreatedAt:dd/MM/yyyy HH:mm}  |  {order.TotalPrice}$",
                    ConsoleColor.Cyan
                );
                foreach (var item in order.Items)
                {
                    var game = _gameService.GetById(item.GameId);
                    UIHelper.WriteLineCentered(
                        $"   └ {game?.Name ?? "Unknown",-20} x{item.Quantity}  {item.PriceAtPurchase}$",
                        ConsoleColor.White
                    );
                }
                UIHelper.WriteLineCentered(new string('─', 55), ConsoleColor.DarkGray);
            }
        }

        UIHelper.Info("Press any key to go back...");
        Console.ReadKey();
    }
}