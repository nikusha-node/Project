using Project.Helpers;
using Project.Services.Interfaces;
using Project.Exceptions;

namespace Project.UserMenus;

public class ShopMenu
{
    private readonly IGameService _gameService;
    private readonly ICartService _cartService;
    private Project.Enums.Genre? _genreFilter = null;

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

            if (_genreFilter.HasValue)
            {
                games = games.Where(g => g.Genre == _genreFilter.Value).ToList();
                UIHelper.WriteLineCentered($"🔍  Filtering by: {_genreFilter}", ConsoleColor.Magenta);
            }

            if (!games.Any())
            {
                UIHelper.Warning("No games available!");
            }
            else
            {
                UIHelper.WriteLineCentered(
                    $"┌─────┬──────────────────────┬───────────────┬───────────┬─────────┐",
                    ConsoleColor.DarkCyan
                );
                UIHelper.WriteLineCentered(
                    $"│ {"ID",-3} │ {"NAME",-20} │ {"GENRE",-13} │ {"PRICE",-9} │ {"STOCK",-7} │",
                    ConsoleColor.DarkBlue
                );
                UIHelper.WriteLineCentered(
                    $"├─────┼──────────────────────┼───────────────┼───────────┼─────────┤",
                    ConsoleColor.DarkCyan
                );

                foreach (var g in games)
                {
                    var rowColor = games.ToList().IndexOf(g) % 2 == 0
                        ? ConsoleColor.Blue
                        : ConsoleColor.Cyan;

                    var stockColor = g.Stock <= 3 ? ConsoleColor.Red : rowColor;

                    UIHelper.WriteLineCentered(
                        $"│ {g.Id,-3} │ {g.Name,-20} │ {g.Genre,-13} │ {g.Price,8}$ │ {g.Stock,-7} │",
                        g.Stock <= 3 ? ConsoleColor.Red : rowColor
                    );
                }

                UIHelper.WriteLineCentered(
                    $"└─────┴──────────────────────┴───────────────┴───────────┴─────────┘",
                    ConsoleColor.DarkCyan
                );

                UIHelper.WriteLineCentered(
                    $"🎮  {games.Count()} game(s) available",
                    ConsoleColor.Green
                );
            }

            UIHelper.Divider();
            UIHelper.WriteLineCentered("F.  🔍  Filter by Genre", ConsoleColor.Magenta);
            UIHelper.WriteLineCentered("0.  🚪  Back", ConsoleColor.DarkGray);
            UIHelper.Divider();

            var input = UIHelper.ReadLineCentered("🛒  Enter Game ID to add to cart (F = filter, 0 = back): ", ConsoleColor.Cyan);

            if (input == "0") return;

            if (input?.ToLower() == "f")
            {
                ShowGenreFilter();
                continue;
            }

            if (int.TryParse(input, out int gameId))
            {
                var game = games.FirstOrDefault(g => g.Id == gameId);
                if (game == null)
                {
                    UIHelper.Error("Game not found!");
                }
                else if (game.Stock <= 0)
                {
                    UIHelper.Error("This game is out of stock!");
                }
                else
                {
                    try
                    {
                        _cartService.AddToCart(gameId, 1);
                        UIHelper.Success($"'{game.Name}' added to cart! 🎉");
                    }
                    catch (NotFoundException)
                    {
                        UIHelper.Error("Game not found!");
                    }
                    catch (Exception ex)
                    {
                        UIHelper.Error(ex.Message);
                    }
                }
            }
            else
            {
                UIHelper.Error("Invalid input!");
            }

            Console.ReadKey();
        }
    }

    private void ShowGenreFilter()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("🔍  FILTER BY GENRE  🔍", ConsoleColor.Yellow);
        UIHelper.Divider();
        UIHelper.WriteLineCentered("0.  All Genres", ConsoleColor.Cyan);

        foreach (var genre in Enum.GetValues(typeof(Project.Enums.Genre)))
        {
            UIHelper.WriteLineCentered($"{(int)genre}.  {genre}", ConsoleColor.White);
        }

        UIHelper.Divider();
        int choice = InputHelper.ReadInt("Select: ");

        if (choice == 0)
        {
            _genreFilter = null;
            UIHelper.Success("Filter cleared!");
        }
        else
        {
            _genreFilter = (Project.Enums.Genre)choice;
            UIHelper.Success($"Filtering by {_genreFilter}!");
        }

        Console.ReadKey();
    }
}