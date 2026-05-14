using Project.Helpers;
using Project.Services.Interfaces;
using Project.Exceptions;

namespace Project.UserMenus;

public class ShopMenu
{
    private readonly IGameService _gameService;
    private readonly ICartService _cartService;
    private readonly IAuthService _authService;
    private readonly IOrderService _orderService;
    private Project.Enums.Genre? _genreFilter = null;
    private int _currentPage = 1;
    private const int PAGE_SIZE = 8;

    public ShopMenu(IGameService gameService, ICartService cartService, IAuthService authService, IOrderService orderService)
    {
        _gameService = gameService;
        _cartService = cartService;
        _authService = authService;
        _orderService = orderService;
    }

    public void Show()
    {
        _currentPage = 1;
        while (true)
        {
            Console.Clear();
            LogoHelper.ShowLogo();

            UIHelper.Divider();
            UIHelper.WriteLineCentered("🛒  GAME SHOP  🛒", ConsoleColor.Yellow);
            UIHelper.Divider();

            var allGames = _gameService.GetAll();
            if (_genreFilter.HasValue)
            {
                allGames = allGames.Where(g => g.Genre == _genreFilter.Value).ToList();
                UIHelper.WriteLineCentered($"🔍  Filter: {_genreFilter}", ConsoleColor.Magenta);
            }

            int totalPages = (int)Math.Ceiling(allGames.Count / (double)PAGE_SIZE);
            if (_currentPage > totalPages && totalPages > 0) _currentPage = totalPages;

            var games = allGames
                .Skip((_currentPage - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();

            if (!allGames.Any())
            {
                UIHelper.Warning("No games available!");
            }
            else
            {
                // page indicator
                UIHelper.WriteLineCentered(
                    $"📄  Page {_currentPage} of {totalPages}  ({allGames.Count} games total)",
                    ConsoleColor.DarkGray
                );

                UIHelper.WriteLineCentered(
                    $"┌─────┬──────────────────────┬───────────────┬───────────┬─────────┬────────┐",
                    ConsoleColor.DarkCyan
                );
                UIHelper.WriteLineCentered(
                    $"│ {"ID",-3} │ {"NAME",-20} │ {"GENRE",-13} │ {"PRICE",-9} │ {"STOCK",-7} │ {"⭐",-6} │",
                    ConsoleColor.DarkBlue
                );
                UIHelper.WriteLineCentered(
                    $"├─────┼──────────────────────┼───────────────┼───────────┼─────────┼────────┤",
                    ConsoleColor.DarkCyan
                );

                foreach (var g in games)
                {
                    var rowColor = g.Stock <= 0
                        ? ConsoleColor.DarkGray
                        : games.IndexOf(g) % 2 == 0 ? ConsoleColor.Blue : ConsoleColor.Cyan;

                    var stars = g.AverageRating > 0 ? $"{g.AverageRating}/5" : "N/A";
                    var stockDisplay = g.Stock <= 0 ? "OUT" : g.Stock <= 3 ? $"⚠️ {g.Stock}" : $"{g.Stock}";

                    UIHelper.WriteLineCentered(
                        $"│ {g.Id,-3} │ {g.Name,-20} │ {g.Genre,-13} │ {g.Price,8}$ │ {stockDisplay,-7} │ {stars,-6} │",
                        rowColor
                    );
                }

                UIHelper.WriteLineCentered(
                    $"└─────┴──────────────────────┴───────────────┴───────────┴─────────┴────────┘",
                    ConsoleColor.DarkCyan
                );

                // pagination controls
                UIHelper.Divider();
                var pageControls = "";
                if (_currentPage > 1) pageControls += "◀  P. Prev  ";
                if (_currentPage < totalPages) pageControls += "N. Next  ▶";
                if (!string.IsNullOrEmpty(pageControls))
                    UIHelper.WriteLineCentered(pageControls, ConsoleColor.DarkYellow);
            }

            UIHelper.Divider();
            UIHelper.WriteLineCentered("F. 🔍 Filter  |  0. 🚪 Back", ConsoleColor.DarkGray);
            UIHelper.Divider();

            var input = UIHelper.ReadLineCentered("🛒  Enter Game ID to add to cart: ", ConsoleColor.Cyan);

            if (input == "0") return;
            if (input?.ToLower() == "f") { ShowGenreFilter(); _currentPage = 1; continue; }
            if (input?.ToLower() == "n" && _currentPage < totalPages) { _currentPage++; continue; }
            if (input?.ToLower() == "p" && _currentPage > 1) { _currentPage--; continue; }

            if (int.TryParse(input, out int gameId))
            {
                // search across ALL pages not just current
                var game = allGames.FirstOrDefault(g => g.Id == gameId);
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
                    int qty = InputHelper.ReadInt("Quantity: ");
                    if (qty <= 0)
                        UIHelper.Error("Invalid quantity!");
                    else if (qty > game.Stock)
                        UIHelper.Error($"Only {game.Stock} in stock!");
                    else
                    {
                        try
                        {
                            _cartService.AddToCart(gameId, qty);
                            UIHelper.Success($"'{game.Name}' x{qty} added to cart! 🎉");
                        }
                        catch (Exception ex)
                        {
                            UIHelper.Error(ex.Message);
                        }
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

    private void RateGame()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("⭐  RATE A GAME  ⭐", ConsoleColor.Yellow);
        UIHelper.Divider();

        var user = _authService.GetCurrentUser();
        var orders = _orderService.GetUserOrders(user.Id);
        var purchasedGameIds = orders.SelectMany(o => o.Items).Select(i => i.GameId).Distinct().ToList();

        if (!purchasedGameIds.Any())
        {
            UIHelper.Warning("You haven't purchased any games yet!");
            Console.ReadKey();
            return;
        }

        UIHelper.WriteLineCentered("Your purchased games:", ConsoleColor.Cyan);
        UIHelper.WriteLineCentered(new string('─', 40), ConsoleColor.DarkGray);

        foreach (var gId in purchasedGameIds)
        {
            var g = _gameService.GetById(gId);
            if (g != null)
                UIHelper.WriteLineCentered($"{g.Id,-5} {g.Name,-22} ⭐ {g.AverageRating}/5", ConsoleColor.White);
        }

        UIHelper.Divider();
        int gameId = InputHelper.ReadInt("Game ID to rate: ");

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

    private void ShowGenreFilter()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("🔍  FILTER BY GENRE  🔍", ConsoleColor.Yellow);
        UIHelper.Divider();
        UIHelper.WriteLineCentered("0.  All Genres", ConsoleColor.Cyan);

        foreach (var genre in Enum.GetValues(typeof(Project.Enums.Genre)))
            UIHelper.WriteLineCentered($"{(int)genre}.  {genre}", ConsoleColor.White);

        UIHelper.Divider();
        int choice = InputHelper.ReadInt("Select: ");

        if (choice == 0) { _genreFilter = null; UIHelper.Success("Filter cleared!"); }
        else { _genreFilter = (Project.Enums.Genre)choice; UIHelper.Success($"Filtering by {_genreFilter}!"); }

        Console.ReadKey();
    }
}