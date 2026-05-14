namespace Project.Models;

public class Rating
{
    public int UserId { get; set; }
    public int Stars { get; set; } 
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}