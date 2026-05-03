namespace Project.Models;

public class Game
{
    public int Id { get; set; }  
    public string Name { get; set; }
    public Enums.Genre Genre { get; set; }
    public decimal Price { get; set; }
}
