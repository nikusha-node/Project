using System.Collections.Generic;
using Project.Models;

namespace Project.Services.Interfaces;

public interface IGameService
{
    List<Game> GetAll();
    Game GetById(int id);

    void Add(Game game);
    void Delete(int id);
}
