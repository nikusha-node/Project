using Project.Exceptions;
using Project.Extensions;
using Project.Helpers;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Admin;

public class AdminMenu
{
    private readonly IGameService _gameService;

    public AdminMenu(IGameService gameService)
    {
        _gameService = gameService;
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
        Console.Clear();
        LogoHelper.ShowLogo();

        UIHelper.Divider();
        UIHelper.WriteLineCentered("🎮  ALL GAMES  🎮", ConsoleColor.Yellow);
        UIHelper.Divider();

        var games = _gameService.GetAll();

        if (!games.Any())
        {
            UIHelper.Warning("No games found!");
        }
        else
        {
            UIHelper.WriteLineCentered(
                $"{"ID",-5} {"NAME",-22} {"GENRE",-15} {"PRICE",-10}",
                ConsoleColor.Magenta
            );
            UIHelper.WriteLineCentered(new string('─', 55), ConsoleColor.DarkGray);

            foreach (var game in games)
            {
                UIHelper.WriteLineCentered(
                    $"{game.Id,-5} {game.Name,-22} {game.Genre,-15} {game.Price,-10}$"
                );
            }
        }

        UIHelper.Divider();
        UIHelper.Info("Press any key to go back...");
        Console.ReadKey();
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
        {
            UIHelper.WriteLineCentered($"  {(int)genre}  ─  {genre}", ConsoleColor.White);
        }

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
        }
        else
        {
            try
            {
                _gameService.Delete(id);
                UIHelper.Success("Game deleted successfully!");
            }
            catch (NotFoundException ex)
            {
                UIHelper.Error(ex.Message);
            }
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
                $"{"ID",-5} {"NAME",-22} {"GENRE",-15} {"PRICE",-10}",
                ConsoleColor.Magenta
            );
            UIHelper.WriteLineCentered(new string('─', 55), ConsoleColor.DarkGray);

            foreach (var game in games)
            {
                UIHelper.WriteLineCentered(
                    $"{game.Id,-5} {game.Name,-22} {game.Genre,-15} {game.Price,-10}$"
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

        UIHelper.WriteLineCentered($"🎮  Total Games  :  {games.Count()}", ConsoleColor.Cyan);

        var mostExpensive = games.OrderByDescending(g => g.Price).FirstOrDefault();
        if (mostExpensive != null)
            UIHelper.WriteLineCentered(
                $"💎  Most Expensive  :  {mostExpensive.Name} ({mostExpensive.Price}$)",
                ConsoleColor.Cyan
            );

        decimal totalPrice = games.Aggregate(0m, (sum, g) => sum + g.Price);
        UIHelper.WriteLineCentered($"💰  Total Value  :  {totalPrice}$", ConsoleColor.Cyan);

        UIHelper.Divider();
        UIHelper.WriteLineCentered("📂  Games By Genre:", ConsoleColor.Magenta);
        UIHelper.WriteLineCentered(new string('─', 30), ConsoleColor.DarkGray);

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
}