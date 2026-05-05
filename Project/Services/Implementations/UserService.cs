using Project.Enums;
using Project.Services.Interfaces;
using Project.Models;
namespace Project.Services.Implementations;

public class UserService : IUserService
{
    private readonly List<User> _users = new();

    public List<User> GetAll() => _users;

    public User GetById(int id)
        => _users.FirstOrDefault(u => u.Id == id);

    public User Create(string username)
    {
        var user = new User
        {
            Id = _users.Count + 1,
            Username = username,
            Role = UserRole.Customer
        };

        _users.Add(user);
        return user;
    }
}
