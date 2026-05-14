using Project.Data;
using Project.Enums;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services.Implementations
{
    public class UserService : IUserService, IRepository<User>
    {
        private readonly DatabaseContext _db;
        private const string PATH = "users.json";

        public UserService(DatabaseContext db)
        {
            _db = db;
            var data = FileHandler.LoadFromFile<List<User>>(PATH);
            if (data != null)
                _db.Users = data;
        }

        private User CreateUser(string username, string password, UserRole role)
        {

            if (_db.Users.Any(u => u.Username.ToLower() == username.ToLower()))
                throw new Exception($"Username '{username}' is already taken!");


            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("Username cannot be empty!");

            var user = new User
            {
                Id = _db.Users.Any() ? _db.Users.Max(u => u.Id) + 1 : 1,
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role
            };

            _db.Users.Add(user);
            Save();
            return user;
        }

        public User Create(string username, string password)
            => CreateUser(username, password, UserRole.Customer);

        public User CreateAdmin(string username, string password)
            => CreateUser(username, password, UserRole.Admin);

        public List<User> GetAll() => _db.Users;

        public User? GetById(int id)
            => _db.Users.FirstOrDefault(u => u.Id == id);

        public void Add(User user)
        {
            _db.Users.Add(user);
            Save();
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                Save();
            }
        }

        private void Save() => FileHandler.SaveToFile(PATH, _db.Users);
    }
}