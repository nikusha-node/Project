using Project.Helpers;
using Project.Services.Interfaces;

namespace Project.UserMenus;

public class UserMenu
{
    private readonly ShopMenu _shopMenu;
    private readonly CartMenu _cartMenu;
    private readonly IGameService _gameService;
    private readonly IAuthService _authService;
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public UserMenu(ShopMenu shopMenu, CartMenu cartMenu, IGameService gameService, IAuthService authService, IOrderService orderService, ICartService cartService) // 👈 add parameter
    {
        _shopMenu = shopMenu;
        _cartMenu = cartMenu;
        _gameService = gameService;
        _authService = authService;
        _orderService = orderService;
        _cartService = cartService; 
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            LogoHelper.ShowLogo();

            var cart = _cartService.GetCart();
            var itemCount = cart.Items.Sum(i => i.Quantity);
            var cartLabel = itemCount > 0
                ? $"2.  🧾  Cart  ({itemCount} item{(itemCount == 1 ? "" : "s")} — {cart.TotalPrice}$)"
                : "2.  🧾  Cart  (empty)";
            var cartColor = itemCount > 0 ? ConsoleColor.Green : ConsoleColor.DarkGray;

            UIHelper.Divider();
            UIHelper.WriteLineCentered("🎮  USER MENU  🎮", ConsoleColor.Yellow);
            UIHelper.Divider();
            UIHelper.WriteLineCentered("1.  🛒  Shop", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered(cartLabel, cartColor); 
            UIHelper.WriteLineCentered("3.  ⭐  Rate a Game", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("0.  🚪  Back", ConsoleColor.DarkGray);
            UIHelper.Divider();

            var choice = UIHelper.ReadLineCentered("Enter your choice: ", ConsoleColor.Cyan);

            switch (choice)
            {
                case "1": _shopMenu.Show(); break;
                case "2": _cartMenu.Show(); break;
                case "3": ShowRateGame(); break;
                case "0": return;
                default:
                    UIHelper.Error("Invalid choice!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ShowRateGame()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("⭐  RATE A GAME  ⭐", ConsoleColor.Yellow);
        UIHelper.Divider();

        var user = _authService.GetCurrentUser();
        var orders = _orderService.GetUserOrders(user.Id);
        var purchasedGameIds = orders
            .SelectMany(o => o.Items)
            .Select(i => i.GameId)
            .Distinct()
            .ToList();

        if (!purchasedGameIds.Any())
        {
            UIHelper.Warning("You haven't purchased any games yet!");
            Console.ReadKey();
            return;
        }

        UIHelper.WriteLineCentered("Your purchased games:", ConsoleColor.Cyan);
        UIHelper.WriteLineCentered(new string('─', 50), ConsoleColor.DarkGray);
        UIHelper.WriteLineCentered(
            $"{"ID",-5} {"NAME",-22} {"YOUR RATING",-15} {"AVG RATING",-10}",
            ConsoleColor.Magenta
        );
        UIHelper.WriteLineCentered(new string('─', 50), ConsoleColor.DarkGray);

        foreach (var gId in purchasedGameIds)
        {
            var g = _gameService.GetById(gId);
            if (g != null)
            {
                var myRating = g.Ratings.FirstOrDefault(r => r.UserId == user.Id);
                var myStars = myRating != null ? $"⭐ {myRating.Stars}/5" : "Not rated";
                var avgStars = g.AverageRating > 0 ? $"⭐ {g.AverageRating}/5" : "No ratings";

                UIHelper.WriteLineCentered(
                    $"{g.Id,-5} {g.Name,-22} {myStars,-15} {avgStars,-10}",
                    ConsoleColor.White
                );
            }
        }

        UIHelper.Divider();
        int gameId = InputHelper.ReadInt("Game ID to rate (0 = back): ");
        if (gameId == 0) return;

        if (!purchasedGameIds.Contains(gameId))
        {
            UIHelper.Error("You can only rate games you've purchased!");
            Console.ReadKey();
            return;
        }

        int stars = InputHelper.ReadInt("Stars (1-5): ");
        if (stars < 1 || stars > 5)
        {
            UIHelper.Error("Stars must be between 1 and 5!");
            Console.ReadKey();
            return;
        }

        UIHelper.Info("Comment (press Enter to skip): ");
        var comment = Console.ReadLine() ?? string.Empty;

        _gameService.AddRating(gameId, user.Id, stars, comment);
        UIHelper.Success("Rating submitted! ⭐");
        Console.ReadKey();
    }
}