namespace Project.Models;

public class Game : BaseEntity
{
    
    public string Name { get; set; } = string.Empty;
    public Enums.Genre Genre { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; } = 10;
    public List<Rating> Ratings { get; set; } = new();
    public double AverageRating => Ratings.Any()
        ? Math.Round(Ratings.Average(r => r.Stars), 1)
        : 0;

    public override string GetInfo()
    {
        return $"{Name} - {Price}$";
    }
}
