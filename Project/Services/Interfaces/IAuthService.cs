using Project.Models;

namespace Project.Services.Interfaces
{
    public interface IAuthService
    {
        User Register(string username, string password);
        User? Login(string username, string password);
        User? GetCurrentUser();
        void Logout();
    }
}