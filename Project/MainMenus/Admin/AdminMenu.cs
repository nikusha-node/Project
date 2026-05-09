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

            Console.WriteLine("=================================");
            Console.WriteLine("         ADMIN PANEL");
            Console.WriteLine("=================================");
            Console.WriteLine("1. View Games");
            Console.WriteLine("2. Add Game");
            Console.WriteLine("3. Delete Game");
            Console.WriteLine("4. Search Game");
            Console.WriteLine("5. Statistics");
            Console.WriteLine("0. Back");
            Console.WriteLine("=================================");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowGames();
                    break;

                case "2":
                    AddGame();
                    break;

                case "3":
                    DeleteGame();
                    break;

                case "4":
                    SearchGame();
                    break;

                case "5":
                    ShowStatistics();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice!");
                    Pause();
                    break;
            }
        }
    }

    private void ShowGames()
    {
        Console.Clear();

        var games = _gameService.GetAll();

        Console.WriteLine("=========== GAMES ===========");

        if (!games.Any())
        {
            Console.WriteLine("No games found.");
        }
        else
        {
            foreach (var game in games)
            {
                Console.WriteLine(
                    $"ID: {game.Id} | " +
                    $"Name: {game.Name} | " +
                    $"Genre: {game.Genre} | " +
                    $"Price: {game.Price}$"
                );
            }
        }

        Pause();
    }

    private void AddGame()
    {
        Console.Clear();

        Console.WriteLine("=========== ADD GAME ===========");

        string name = InputHelper.ReadString("Game Name: ").Capitalize();
        decimal price = InputHelper.ReadDecimal("Price: ");

        Console.WriteLine("\nSelect Genre:");

        foreach (var genre in Enum.GetValues(typeof(Project.Enums.Genre)))
        {
            Console.WriteLine($"{(int)genre} - {genre}");
        }

        int genreChoice = InputHelper.ReadInt("Genre Number: ");

        var selectedGenre = (Project.Enums.Genre)genreChoice;

        var game = new Game
        {
            Name = name,
            Price = price,
            Genre = selectedGenre
        };

        _gameService.Add(game);

        Console.WriteLine("\nGame added successfully!");

        Pause();
    }

    private void DeleteGame()
    {
        Console.Clear();

        Console.WriteLine("=========== DELETE GAME ===========");

        int id = InputHelper.ReadInt("Game Id: ");

        var game = _gameService.GetById(id);

        if (game == null)
        {
            Console.WriteLine("Game not found!");
        }
        else
        {
            try
            {
                _gameService.Delete(id);
                Console.WriteLine("Game deleted successfully!");
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        Pause();
    }

    private void SearchGame()
    {
        Console.Clear();

        Console.WriteLine("=========== SEARCH GAME ===========");

        string keyword = InputHelper.ReadString("Enter game name: ");

        var games = _gameService.GetAll()
            .Where(g => g.Name.ToLower().Contains(keyword.ToLower()))
            .ToList();

        if (!games.Any())
        {
            Console.WriteLine("No games found.");
        }
        else
        {
            foreach (var game in games)
            {
                Console.WriteLine(
                    $"ID: {game.Id} | " +
                    $"Name: {game.Name} | " +
                    $"Genre: {game.Genre} | " +
                    $"Price: {game.Price}$"
                );
            }
        }

        Pause();
    }

    private void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }


    private void ShowStatistics()
    {
        Console.Clear();

        var games = _gameService.GetAll();

        Console.WriteLine("=========== STATISTICS ===========");

        bool hasGames = games.Any();

        Console.WriteLine($"Has Games: {hasGames}");

        var mostExpensive = games
            .OrderByDescending(g => g.Price)
            .FirstOrDefault();

        if (mostExpensive != null)
        {
            Console.WriteLine(
                $"Most Expensive Game: {mostExpensive.Name} ({mostExpensive.Price}$)"
            );
        }

        var groupedGenres = games
            .GroupBy(g => g.Genre);

        Console.WriteLine("\nGames By Genre:");

        foreach (var group in groupedGenres)
        {
            Console.WriteLine($"{group.Key}: {group.Count()} games");
        }

        decimal totalPrice = games
            .Aggregate(0m, (sum, game) => sum + game.Price);

        Console.WriteLine($"\nTotal Games Price: {totalPrice}$");

        Pause();
    }
}