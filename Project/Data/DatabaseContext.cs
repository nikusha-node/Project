using Project.Models;

namespace Project.Data;

public class DatabaseContext
{
    public List<Game> Games { get; set; } = SeedData.GetGames();

    public List<User> Users { get; set; } = new();

    public List<Order> Orders { get; set; } = new();
}