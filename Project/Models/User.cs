namespace Project.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public Enums.UserRole Role { get; set; }
}
