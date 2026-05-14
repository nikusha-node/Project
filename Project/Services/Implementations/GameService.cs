using Project.Data;
using Project.Models;
using Project.Services.Interfaces;
using Project.Exceptions;

namespace Project.Services.Implementations;

public class GameService : IGameService, IRepository<Game>
{
    private readonly DatabaseContext _db;
    private const string PATH = "games.json";

    public GameService(DatabaseContext db)
    {
        _db = db;

        var data = FileHandler.LoadFromFile<List<Game>>(PATH);

        if (data != null && data.Any())
        {
            _db.Games = data;
        }
        else
        {
            Save();
        }
    }

    public List<Game> GetAll() => _db.Games;


    public Game? GetById(int id)
    {
        return _db.Games.FirstOrDefault(g => g.Id == id);
    }

    public void Add(Game game) 
    {
        game.Id = _db.Users.Any() ? _db.Users.Max(u => u.Id) + 1 : 1;
        _db.Games.Add(game);
        Save();
    }

    public void Delete(int id)
    {
        var game = _db.Games.FirstOrDefault(g => g.Id == id);

        if (game == null)
        {
            throw new NotFoundException();
        }

        _db.Games.Remove(game);

        Save();
    }

    public void Update(Game game)
    {
        var existing = _db.Games.FirstOrDefault(g => g.Id == game.Id);
        if (existing == null) throw new NotFoundException();

        existing.Name = game.Name;
        existing.Price = game.Price;
        existing.Genre = game.Genre;

        Save();
    }

    private void Save()
    {
        FileHandler.SaveToFile(PATH, _db.Games);
    }
}