using Project.Data;
using Project.Enums;
using Project.Models;
using Project.Services.Interfaces;
using System.Linq;

namespace Project.Services.Implementations
{
    public class UserService : IUserService
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

        public User Create(string username)
        {
            var user = new User
            {
                Id = _db.Users.Count + 1,
                Username = username,
                Role = UserRole.Customer
            };

            _db.Users.Add(user);

            Save();

            return user;
        }

        public User CreateAdmin(string username)
        {
            var user = new User
            {
                Id = _db.Users.Count + 1,
                Username = username,
                Role = UserRole.Admin
            };

            _db.Users.Add(user);

            Save();

            return user;
        }

        public List<User> GetAll()
        {
            return _db.Users;
        }

        public User? GetById(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        private void Save()
        {
            FileHandler.SaveToFile(PATH, _db.Users);
        }
    }
}