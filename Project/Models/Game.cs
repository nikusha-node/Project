namespace Project.Models;

public class Game : BaseEntity
{
    public int Id { get; set; }  
    public string Name { get; set; }
    public Enums.Genre Genre { get; set; }
    public decimal Price { get; set; }


    public override string GetInfo()
    {
        return $"{Name} - {Price}$";
    }
}
