using System.Collections.Generic;
using Project.Models;

namespace Project.Services.Interfaces;

public interface IUserService
{
    List<User> GetAll();
    User GetById(int id);

    User Create(string username);
}
