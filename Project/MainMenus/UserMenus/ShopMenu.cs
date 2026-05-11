using Project.Helpers;
using Project.Services.Interfaces;

namespace Project.UserMenus;

public class ShopMenu
{
    private readonly IGameService _gameService;
    private readonly ICartService _cartService;

    public ShopMenu(IGameService gameService, ICartService cartService)
    {
        _gameService = gameService;
        _cartService = cartService;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            LogoHelper.ShowLogo();

            UIHelper.Divider();
            UIHelper.WriteLineCentered("🛒  GAME SHOP  🛒", ConsoleColor.Yellow);
            UIHelper.Divider();

            var games = _gameService.GetAll();

            if (!games.Any())
            {
                UIHelper.Warning("No games available yet!");
            }
            else
            {
 
                UIHelper.WriteLineCentered(
                    $"┌─────┬──────────────────────┬───────────────┬───────────┐",
                    ConsoleColor.DarkCyan
                );
                UIHelper.WriteLineCentered(
                    $"│ {"ID",-3} │ {"NAME",-20} │ {"GENRE",-13} │ {"PRICE",-9} │",
                    ConsoleColor.DarkBlue
                );
                UIHelper.WriteLineCentered(
                    $"├─────┼──────────────────────┼───────────────┼───────────┤",
                    ConsoleColor.DarkCyan
                );

                foreach (var g in games)
                {
                    var rowColor = games.ToList().IndexOf(g) % 2 == 0
                        ? ConsoleColor.Blue
                        : ConsoleColor.Cyan;

                    UIHelper.WriteLineCentered(
                        $"│ {g.Id,-3} │ {g.Name,-20} │ {g.Genre,-13} │ {g.Price,8}$ │",
                        rowColor
                    );
                }

                UIHelper.WriteLineCentered(
                    $"└─────┴──────────────────────┴───────────────┴───────────┘",
                    ConsoleColor.DarkCyan
                );

                UIHelper.WriteLineCentered(
                    $"🎮  {games.Count()} game(s) available",
                    ConsoleColor.Green
                );
            }

            UIHelper.Divider();

            var input = UIHelper.ReadLineCentered("🛒  Enter Game ID to add to cart (0 = back): ", ConsoleColor.Cyan);
            if (input == "0") return;

            if (int.TryParse(input, out int gameId))
            {
                var game = games.FirstOrDefault(g => g.Id == gameId);
                if (game == null)
                {
                    UIHelper.Error("Game not found!");
                }
                else
                {
                    _cartService.AddToCart(gameId, 1);
                    UIHelper.Success($"'{game.Name}' added to cart! 🎉");
                }
            }
            else
            {
                UIHelper.Error("Invalid ID!");
            }
        }
    }
}