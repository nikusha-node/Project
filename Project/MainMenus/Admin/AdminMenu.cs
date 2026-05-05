using Project.Models;
using Project.Services.Interfaces;
using Project.Helpers;

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
            Console.WriteLine("\n=== ADMIN PANEL ===");
            Console.WriteLine("1. View Games");
            Console.WriteLine("2. Add Game");
            Console.WriteLine("3. Delete Game");
            Console.WriteLine("0. Back");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    foreach (var g in _gameService.GetAll())
                        Console.WriteLine($"{g.Id}. {g.Name} - {g.Price}$");
                    break;

                case "2":
                    var name = InputHelper.ReadString("Name: ");
                    var price = InputHelper.ReadDecimal("Price: ");

                    _gameService.Add(new Game
                    {
                        Name = name,
                        Price = price,
                        Genre = Enums.Genre.Action
                    });
                    break;

                case "3":
                    int id = InputHelper.ReadInt("Id: ");
                    _gameService.Delete(id);
                    break;

                case "0":
                    return;
            }
        }
    }
}