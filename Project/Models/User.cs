namespace Project.Models;

using Project.Enums;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public override string GetInfo() => $"{Username} - {Role}";
}
