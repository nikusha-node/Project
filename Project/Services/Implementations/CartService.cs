using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services.Implementations;

public class CartService : ICartService
{
    private readonly Cart _cart = new();
    private readonly IGameService _gameService;

    public CartService(IGameService gameService)
    {
        _gameService = gameService;
    }

    public Cart GetCart() => _cart;

    public void AddToCart(int gameId, int quantity)
    {
        var game = _gameService.GetById(gameId);
        if (game == null) return;

        var existing = _cart.Items.FirstOrDefault(i => i.GameId == gameId);

        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            _cart.Items.Add(new OrderItem
            {
                GameId = gameId,
                Quantity = quantity,
                PriceAtPurchase = game.Price
            });
        }
    }

    public void RemoveFromCart(int gameId)
    {
        var item = _cart.Items.FirstOrDefault(i => i.GameId == gameId);
        if (item != null)
            _cart.Items.Remove(item);
    }

    public void Clear()
    {
        _cart.Clear();
    }
}
