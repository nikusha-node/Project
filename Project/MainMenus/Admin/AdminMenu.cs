using Project.Exceptions;
using Project.Extensions;
using Project.Helpers;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Admin;

public class AdminMenu
{
    private readonly IGameService _gameService;
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;

    public AdminMenu(IGameService gameService, IOrderService orderService, IUserService userService)
    {
        _gameService = gameService;
        _orderService = orderService;
        _userService = userService;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            LogoHelper.ShowLogo();
            UIHelper.Divider();
            UIHelper.WriteLineCentered("🛡️   ADMIN PANEL  🛡️", ConsoleColor.Yellow);
            UIHelper.Divider();
            UIHelper.WriteLineCentered("1.  🎮  View Games", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("2.  ➕  Add Game", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("3.  🗑️   Delete Game", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("4.  🔍  Search Game", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("5.  📊  Statistics", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("6.  ✏️   Edit Game", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("7.  📦  View All Orders", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("8.  👥  User Management", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("0.  🚪  Back", ConsoleColor.DarkGray);
            UIHelper.Divider();

            var choice = UIHelper.ReadLineCentered("Enter your choice: ", ConsoleColor.Cyan);

            switch (choice)
            {
                case "1": ShowGames(); break;
                case "2": AddGame(); break;
                case "3": DeleteGame(); break;
                case "4": SearchGame(); break;
                case "5": ShowStatistics(); break;
                case "6": EditGame(); break;
                case "7": ShowAllOrders(); break;
                case "8": ShowUserManagement(); break;
                case "0": return;
                default:
                    UIHelper.Error("Invalid choice!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ShowGames()
    {
        int page = 1;
        const int PAGE_SIZE = 8;

        while (true)
        {
            Console.Clear();
            LogoHelper.ShowLogo();
            UIHelper.Divider();
            UIHelper.WriteLineCentered("🎮  ALL GAMES  🎮", ConsoleColor.Yellow);
            UIHelper.Divider();

            var allGames = _gameService.GetAll();

            if (!allGames.Any())
            {
                UIHelper.Warning("No games found!");
                Console.ReadKey();
                return;
            }

            int totalPages = (int)Math.Ceiling(allGames.Count / (double)PAGE_SIZE);
            if (page > totalPages) page = totalPages;

            var games = allGames.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();

            UIHelper.WriteLineCentered(
                $"📄  Page {page} of {totalPages}  ({allGames.Count} games total)",
                ConsoleColor.DarkGray
            );
            UIHelper.WriteLineCentered(
                $"{"ID",-5} {"NAME",-22} {"GENRE",-15} {"PRICE",-10} {"STOCK",-10} {"⭐",-8}",
                ConsoleColor.Magenta
            );
            UIHelper.WriteLineCentered(new string('─', 73), ConsoleColor.DarkGray);

            foreach (var game in games)
            {
                var stars = game.AverageRating > 0 ? $"{game.AverageRating}/5" : "N/A";
                UIHelper.WriteLineCentered(
                    $"{game.Id,-5} {game.Name,-22} {game.Genre,-15} {game.Price,-10}$ {game.Stock,-10} {stars,-8}",
                    ConsoleColor.White
                );
            }

            UIHelper.Divider();
            var controls = "";
            if (page > 1) controls += "P. ◀ Prev  ";
            if (page < totalPages) controls += "N. Next ▶";
            if (!string.IsNullOrEmpty(controls))
                UIHelper.WriteLineCentered(controls, ConsoleColor.DarkYellow);
            UIHelper.WriteLineCentered("0.  🚪  Back", ConsoleColor.DarkGray);
            UIHelper.Divider();

            var input = UIHelper.ReadLineCentered("Enter choice: ", ConsoleColor.Cyan);
            if (input == "0") return;
            if (input?.ToLower() == "n" && page < totalPages) { page++; continue; }
            if (input?.ToLower() == "p" && page > 1) { page--; continue; }
        }
    }

    private void AddGame()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("➕  ADD GAME  ➕", ConsoleColor.Yellow);
        UIHelper.Divider();

        string name = InputHelper.ReadString("Game Name: ").Capitalize();
        decimal price = InputHelper.ReadDecimal("Price: ");

        UIHelper.WriteLineCentered("Select Genre:", ConsoleColor.Cyan);
        UIHelper.WriteLineCentered(new string('─', 30), ConsoleColor.DarkGray);

        foreach (var genre in Enum.GetValues(typeof(Project.Enums.Genre)))
            UIHelper.WriteLineCentered($"  {(int)genre}  ─  {genre}", ConsoleColor.White);

        UIHelper.Divider();
        int genreChoice = InputHelper.ReadInt("Genre Number: ");
        var selectedGenre = (Project.Enums.Genre)genreChoice;

        _gameService.Add(new Game
        {
            Name = name,
            Price = price,
            Genre = selectedGenre
        });

        UIHelper.Success("Game added successfully!");
        Console.ReadKey();
    }

    private void DeleteGame()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("🗑️   DELETE GAME  🗑️", ConsoleColor.Yellow);
        UIHelper.Divider();

        int id = InputHelper.ReadInt("Game ID: ");
        var game = _gameService.GetById(id);

        if (game == null)
        {
            UIHelper.Error("Game not found!");
            Console.ReadKey();
            return;
        }

        UIHelper.Warning($"Are you sure you want to delete '{game.Name}'? (Y/N)");
        var confirm = Console.ReadLine()?.ToLower();
        if (confirm != "y")
        {
            UIHelper.Info("Cancelled.");
            Console.ReadKey();
            return;
        }

        try
        {
            _gameService.Delete(id);
            UIHelper.Success("Game deleted successfully!");
        }
        catch (NotFoundException ex)
        {
            UIHelper.Error(ex.Message);
        }

        Console.ReadKey();
    }

    private void SearchGame()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("🔍  SEARCH GAME  🔍", ConsoleColor.Yellow);
        UIHelper.Divider();

        string keyword = InputHelper.ReadString("Search: ");

        var games = _gameService.GetAll()
            .Where(g => g.Name.ToLower().Contains(keyword.ToLower()))
            .ToList();

        if (!games.Any())
        {
            UIHelper.Warning("No games found!");
        }
        else
        {
            UIHelper.WriteLineCentered(
                $"{"ID",-5} {"NAME",-22} {"GENRE",-15} {"PRICE",-10} {"STOCK",-10}",
                ConsoleColor.Magenta
            );
            UIHelper.WriteLineCentered(new string('─', 65), ConsoleColor.DarkGray);

            foreach (var game in games)
            {
                UIHelper.WriteLineCentered(
                    $"{game.Id,-5} {game.Name,-22} {game.Genre,-15} {game.Price,-10}$ {game.Stock,-10}",
                    ConsoleColor.White
                );
            }
        }

        UIHelper.Divider();
        UIHelper.Info("Press any key to go back...");
        Console.ReadKey();
    }

    private void ShowStatistics()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("📊  STATISTICS  📊", ConsoleColor.Yellow);
        UIHelper.Divider();

        var games = _gameService.GetAll();
        var orders = _orderService.GetAll();
        var users = _userService.GetAll();

        UIHelper.WriteLineCentered("📈  GENERAL", ConsoleColor.Magenta);
        UIHelper.WriteLineCentered(new string('─', 45), ConsoleColor.DarkGray);
        UIHelper.WriteLineCentered($"🎮  Total Games     :  {games.Count}", ConsoleColor.Cyan);
        UIHelper.WriteLineCentered($"👥  Total Users     :  {users.Count}", ConsoleColor.Cyan);
        UIHelper.WriteLineCentered($"📦  Total Orders    :  {orders.Count}", ConsoleColor.Cyan);
        UIHelper.WriteLineCentered($"💰  Total Revenue   :  {orders.Sum(o => o.TotalPrice)}$", ConsoleColor.Green);

        UIHelper.Divider();

        UIHelper.WriteLineCentered("🎮  GAMES", ConsoleColor.Magenta);
        UIHelper.WriteLineCentered(new string('─', 45), ConsoleColor.DarkGray);

        var mostExpensive = games.OrderByDescending(g => g.Price).FirstOrDefault();
        var cheapest = games.OrderBy(g => g.Price).FirstOrDefault();
        var topRated = games.Where(g => g.Ratings.Any())
            .OrderByDescending(g => g.AverageRating).FirstOrDefault();
        var lowStock = games.Where(g => g.Stock <= 3 && g.Stock > 0).ToList();
        var outOfStock = games.Where(g => g.Stock <= 0).ToList();

        if (mostExpensive != null)
            UIHelper.WriteLineCentered($"💎  Most Expensive  :  {mostExpensive.Name} ({mostExpensive.Price}$)", ConsoleColor.Cyan);
        if (cheapest != null)
            UIHelper.WriteLineCentered($"💸  Cheapest        :  {cheapest.Name} ({cheapest.Price}$)", ConsoleColor.Cyan);
        if (topRated != null)
            UIHelper.WriteLineCentered($"⭐  Top Rated       :  {topRated.Name} ({topRated.AverageRating}/5)", ConsoleColor.Yellow);

        UIHelper.WriteLineCentered($"⚠️   Low Stock       :  {lowStock.Count} game(s)", ConsoleColor.DarkYellow);
        UIHelper.WriteLineCentered($"❌  Out of Stock    :  {outOfStock.Count} game(s)", ConsoleColor.Red);

        UIHelper.Divider();

        UIHelper.WriteLineCentered("🏆  BEST SELLING GAMES", ConsoleColor.Magenta);
        UIHelper.WriteLineCentered(new string('─', 45), ConsoleColor.DarkGray);

        var bestSelling = orders
            .SelectMany(o => o.Items)
            .GroupBy(i => i.GameId)
            .Select(g => new
            {
                GameId = g.Key,
                TotalSold = g.Sum(i => i.Quantity),
                Revenue = g.Sum(i => i.PriceAtPurchase * i.Quantity)
            })
            .OrderByDescending(g => g.TotalSold)
            .Take(5)
            .ToList();

        if (!bestSelling.Any())
        {
            UIHelper.WriteLineCentered("No sales yet!", ConsoleColor.DarkGray);
        }
        else
        {
            UIHelper.WriteLineCentered(
                $"{"#",-4} {"NAME",-22} {"SOLD",-8} {"REVENUE",-10}",
                ConsoleColor.White
            );
            int rank = 1;
            foreach (var item in bestSelling)
            {
                var game = _gameService.GetById(item.GameId);
                var medal = rank == 1 ? "🥇" : rank == 2 ? "🥈" : rank == 3 ? "🥉" : $"#{rank} ";
                UIHelper.WriteLineCentered(
                    $"{medal,-4} {game?.Name ?? "Unknown",-22} {item.TotalSold,-8} {item.Revenue,-10}$",
                    ConsoleColor.White
                );
                rank++;
            }
        }

        UIHelper.Divider();

        UIHelper.WriteLineCentered("📂  GAMES BY GENRE", ConsoleColor.Magenta);
        UIHelper.WriteLineCentered(new string('─', 45), ConsoleColor.DarkGray);
        foreach (var group in games.GroupBy(g => g.Genre))
        {
            UIHelper.WriteLineCentered(
                $"  {group.Key,-15}  ─  {group.Count()} games",
                ConsoleColor.White
            );
        }

        UIHelper.Divider();
        UIHelper.Info("Press any key to go back...");
        Console.ReadKey();
    }

    private void EditGame()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("✏️   EDIT GAME  ✏️", ConsoleColor.Yellow);
        UIHelper.Divider();

        int id = InputHelper.ReadInt("Game ID to edit: ");
        var game = _gameService.GetById(id);

        if (game == null)
        {
            UIHelper.Error("Game not found!");
            Console.ReadKey();
            return;
        }

        UIHelper.Info($"Current Name: {game.Name} (press Enter to keep)");
        var newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
            game.Name = newName;

        UIHelper.Info($"Current Price: {game.Price}$ (press Enter to keep)");
        var priceInput = Console.ReadLine();
        if (decimal.TryParse(priceInput, out decimal newPrice))
            game.Price = newPrice;

        UIHelper.Info($"Current Stock: {game.Stock} (press Enter to keep)");
        var stockInput = Console.ReadLine();
        if (int.TryParse(stockInput, out int newStock))
            game.Stock = newStock;

        UIHelper.WriteLineCentered("Select new Genre (press Enter to keep):", ConsoleColor.Cyan);
        foreach (var genre in Enum.GetValues(typeof(Project.Enums.Genre)))
            UIHelper.WriteLineCentered($"  {(int)genre}  ─  {genre}", ConsoleColor.White);

        var genreInput = Console.ReadLine();
        if (int.TryParse(genreInput, out int newGenre))
            game.Genre = (Project.Enums.Genre)newGenre;

        _gameService.Update(game);
        UIHelper.Success("Game updated successfully!");
        Console.ReadKey();
    }

    private void ShowAllOrders()
    {
        Console.Clear();
        LogoHelper.ShowLogo();
        UIHelper.Divider();
        UIHelper.WriteLineCentered("📦  ALL ORDERS  📦", ConsoleColor.Yellow);
        UIHelper.Divider();

        var orders = _orderService.GetAll();

        if (!orders.Any())
        {
            UIHelper.Warning("No orders yet!");
        }
        else
        {
            UIHelper.WriteLineCentered($"📦  Total Orders: {orders.Count}", ConsoleColor.Green);
            UIHelper.WriteLineCentered($"💰  Total Revenue: {orders.Sum(o => o.TotalPrice)}$", ConsoleColor.Green);
            UIHelper.Divider();

            foreach (var order in orders)
            {
                var user = _userService.GetById(order.UserId);
                UIHelper.WriteLineCentered(
                    $"Order #{order.Id}  |  👤 {user?.Username ?? "Unknown"}  |  {order.CreatedAt:dd/MM/yyyy HH:mm}  |  {order.TotalPrice}$",
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
                UIHelper.WriteLineCentered(new string('─', 65), ConsoleColor.DarkGray);
            }
        }

        UIHelper.Info("Press any key to go back...");
        Console.ReadKey();
    }

    private void ShowUserManagement()
    {
        while (true)
        {
            Console.Clear();
            LogoHelper.ShowLogo();
            UIHelper.Divider();
            UIHelper.WriteLineCentered("👥  USER MANAGEMENT  👥", ConsoleColor.Yellow);
            UIHelper.Divider();

            var users = _userService.GetAll();

            UIHelper.WriteLineCentered(
                $"{"ID",-5} {"USERNAME",-20} {"ROLE",-15}",
                ConsoleColor.Magenta
            );
            UIHelper.WriteLineCentered(new string('─', 42), ConsoleColor.DarkGray);

            foreach (var user in users)
            {
                var color = user.Role == Enums.UserRole.Admin ? ConsoleColor.Magenta : ConsoleColor.White;
                UIHelper.WriteLineCentered(
                    $"{user.Id,-5} {user.Username,-20} {user.Role,-15}",
                    color
                );
            }

            UIHelper.Divider();
            UIHelper.WriteLineCentered("1.  🗑️   Delete User", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("0.  🚪  Back", ConsoleColor.DarkGray);
            UIHelper.Divider();

            var choice = UIHelper.ReadLineCentered("Enter your choice: ", ConsoleColor.Cyan);

            switch (choice)
            {
                case "1":
                    int id = InputHelper.ReadInt("User ID to delete: ");
                    var user = _userService.GetById(id);
                    if (user == null)
                    {
                        UIHelper.Error("User not found!");
                    }
                    else if (user.Role == Enums.UserRole.Admin)
                    {
                        UIHelper.Error("Cannot delete admin account!");
                    }
                    else
                    {
                        UIHelper.Warning($"Are you sure you want to delete '{user.Username}'? (Y/N)");
                        var confirm = Console.ReadLine()?.ToLower();
                        if (confirm == "y")
                        {
                            _userService.Delete(id);
                            UIHelper.Success($"User '{user.Username}' deleted!");
                        }
                        else
                        {
                            UIHelper.Info("Cancelled.");
                        }
                    }
                    Console.ReadKey();
                    break;
                case "0": return;
                default:
                    UIHelper.Error("Invalid choice!");
                    Console.ReadKey();
                    break;
            }
        }
    }
}