using Project.Models;

namespace Project.Services.Interfaces
{
    public interface IAuthService
    {
        User Register(string username);
        User Login(string username);
        User GetCurrentUser();
    }
}