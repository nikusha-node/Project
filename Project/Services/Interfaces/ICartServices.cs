using Project.Models;

namespace Project.Services.Interfaces;

public interface ICartService
{
    Cart GetCart();

    void AddToCart(int gameId, int quantity);
    void RemoveFromCart(int gameId);

    void Clear();
}

