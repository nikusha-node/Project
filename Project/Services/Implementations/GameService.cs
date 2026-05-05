using System.Collections.Generic;
using System.Linq;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services.Implementations;

public class GameService : IGameService
{
    private readonly List<Game> _games = new();

    public List<Game> GetAll() => _games;

    public Game GetById(int id)
        => _games.FirstOrDefault(g => g.Id == id);

    public void Add(Game game)
    {
        game.Id = _games.Count + 1;
        _games.Add(game);
    }

    public void Delete(int id)
    {
        var game = GetById(id);
        if (game != null)
            _games.Remove(game);
    }
}