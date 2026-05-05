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
            Console.WriteLine("\n=== SHOP ===");

            foreach (var g in _gameService.GetAll())
                Console.WriteLine($"{g.Id}. {g.Name} - {g.Price}$");

            Console.WriteLine("Enter game Id to add to cart (0 to back):");

            var input = Console.ReadLine();
            if (input == "0") return;

            int gameId = int.Parse(input);

            _cartService.AddToCart(gameId, 1);

            Console.WriteLine("Added to cart!");
        }
    }
}