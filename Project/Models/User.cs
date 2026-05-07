namespace Project.Models;

using Project.Enums;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public UserRole Role { get; set; }
}
