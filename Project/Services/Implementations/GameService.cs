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
            _db.Games = data;
        else
            Save();
    }

    public List<Game> GetAll() => _db.Games;

    public Game? GetById(int id) => _db.Games.FirstOrDefault(g => g.Id == id);

    public void Add(Game game)
    {
        game.Id = _db.Games.Any() ? _db.Games.Max(g => g.Id) + 1 : 1;
        _db.Games.Add(game);
        Save();
    }

    public void Delete(int id)
    {
        var game = _db.Games.FirstOrDefault(g => g.Id == id);
        if (game == null) throw new NotFoundException();
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
        existing.Stock = game.Stock; 
        Save();
    }

    public void AddRating(int gameId, int userId, int stars, string comment)
    {
        var game = GetById(gameId);
        if (game == null) throw new NotFoundException();


        var existing = game.Ratings.FirstOrDefault(r => r.UserId == userId);
        if (existing != null)
        {
            existing.Stars = stars;
            existing.Comment = comment;
            existing.CreatedAt = DateTime.Now;
        }
        else
        {
            game.Ratings.Add(new Rating
            {
                UserId = userId,
                Stars = stars,
                Comment = comment,
                CreatedAt = DateTime.Now
            });
        }
        Save();
    }

    private void Save() => FileHandler.SaveToFile(PATH, _db.Games);
}