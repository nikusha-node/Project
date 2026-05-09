using Project.Data;
using Project.Enums;
using Project.Models;
using Project.Services.Interfaces;
using System.Linq;

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

        public User Create(string username, string password)
        {
            var user = new User
            {
                Id = _db.Users.Count + 1,
                Username = username,
                Password = password,
                Role = UserRole.Customer
            };

            _db.Users.Add(user);
            Save();

            return user;
        }

        public User CreateAdmin(string username, string password)
        {
            var user = new User
            {
                Id = _db.Users.Count + 1,
                Username = username,
                Password = password,
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
    }
}