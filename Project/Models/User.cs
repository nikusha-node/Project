namespace Project.Models;

using Project.Enums;

public class User : BaseEntity
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }

   public override string GetInfo()
    {
        return $"{Username} - {Role}";
    }
}
