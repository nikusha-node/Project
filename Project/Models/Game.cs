namespace Project.Models;

public class Game : BaseEntity
{
    
    public string Name { get; set; } = string.Empty;
    public Enums.Genre Genre { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; } = 10;


    public override string GetInfo()
    {
        return $"{Name} - {Price}$";
    }
}
