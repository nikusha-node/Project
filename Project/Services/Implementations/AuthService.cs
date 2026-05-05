using System.Linq;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services.Implementations
{


    public class AuthService : IAuthService
    {

        private readonly IUserService _userService;
        private User _currentUser;

        public AuthService(IUserService userService)
        {
            _userService = userService;
        }

        public User Register(string username)
        {
            var user = _userService.Create(username);
            _currentUser = user;
            return user;
        }

        public User Login(string username)
        {
            var user = _userService.GetAll()
                .FirstOrDefault(u => u.Username == username);

            if (user == null)
                return null;

            _currentUser = user;
            return user;
        }

        public User GetCurrentUser() => _currentUser;



    }
}