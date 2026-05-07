using Project.Data;
using Project.Models;
using Project.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Project.Services.Implementations;

public class GameService : IGameService
{
    private readonly DatabaseContext _db;
    private const string PATH = "games.json";

    public GameService(DatabaseContext db)
    {
        _db = db;

        var data = FileHandler.LoadFromFile<List<Game>>(PATH);

        if (data != null)
            _db.Games = data;
    }

    public List<Game> GetAll() => _db.Games;


    public Game GetById(int id)
        => _db.Games.FirstOrDefault(g => g.Id == id);

    public void Add(Game game) 
    {
        game.Id = _db.Games.Count + 1;
        _db.Games.Add(game);
        Save();
    }

    public void Delete(int id)
    {
        var game = GetById(id);
        if (game != null)
            _db.Games.Remove(game);
    }

    private void Save()
    {
        FileHandler.SaveToFile(PATH, _db.Games);
    }
}