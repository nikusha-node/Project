using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private User? _currentUser;

        public AuthService(IUserService userService)
        {
            _userService = userService;
        }

        public User Register(string username, string password)
        {
            var user = _userService.Create(username, password);
            _currentUser = user;
            return user;
        }

        public User? Login(string username, string password)
        {
            var user = _userService.GetAll()
                .FirstOrDefault(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            _currentUser = user;
            return user;
        }

        public User? GetCurrentUser()
        {
            return _currentUser;
        }

        public void Logout()
        {
            _currentUser = null;
        }
    }
}