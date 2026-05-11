using Project.Models;
using Project.Enums;

namespace Project.Data;

public static class SeedData
{
    public static List<Game> GetGames()
    {
        return new List<Game>
        {
            new Game { Id = 1, Name = "The Witcher 3", Genre = Genre.RPG, Price = 40 },
            new Game { Id = 2, Name = "Cyberpunk 2077", Genre = Genre.RPG, Price = 55 },
            new Game { Id = 3, Name = "GTA V", Genre = Genre.Action, Price = 30 },
            new Game { Id = 4, Name = "RDR 2", Genre = Genre.Action, Price = 60 },
            new Game { Id = 5, Name = "Resident Evil 4", Genre = Genre.Horror, Price = 50 },
            new Game { Id = 6, Name = "Minecraft", Genre = Genre.Sandbox, Price = 25 },
            new Game { Id = 7, Name = "Terraria", Genre = Genre.Sandbox, Price = 15 },
            new Game { Id = 8, Name = "FIFA 25", Genre = Genre.Sports, Price = 70 },
            new Game { Id = 9, Name = "EA FC 25", Genre = Genre.Sports, Price = 70 },
            new Game { Id = 10, Name = "Call of Duty MW3", Genre = Genre.Shooter, Price = 65 },
            new Game { Id = 11, Name = "Counter Strike 2", Genre = Genre.Shooter, Price = 0 },
            new Game { Id = 12, Name = "Valorant", Genre = Genre.Shooter, Price = 0 },
            new Game { Id = 13, Name = "Elden Ring", Genre = Genre.RPG, Price = 60 },
            new Game { Id = 14, Name = "Dark Souls 3", Genre = Genre.RPG, Price = 40 },
            new Game { Id = 15, Name = "Sekiro", Genre = Genre.Action, Price = 50 },
            new Game { Id = 16, Name = "God of War", Genre = Genre.Action, Price = 55 },
            new Game { Id = 17, Name = "God of War Ragnarok", Genre = Genre.Action, Price = 70 },
            new Game { Id = 18, Name = "Hollow Knight", Genre = Genre.Adventure, Price = 20 },
            new Game { Id = 19, Name = "Stardew Valley", Genre = Genre.Simulation, Price = 15 },
            new Game { Id = 20, Name = "Forza Horizon 5", Genre = Genre.Racing, Price = 50 },
            new Game { Id = 21, Name = "Need For Speed Heat", Genre = Genre.Racing, Price = 35 },
            new Game { Id = 22, Name = "Outlast", Genre = Genre.Horror, Price = 20 },
            new Game { Id = 23, Name = "Phasmophobia", Genre = Genre.Horror, Price = 15 },
            new Game { Id = 24, Name = "Detroit Become Human", Genre = Genre.Adventure, Price = 35 },
            new Game { Id = 25, Name = "Rust", Genre = Genre.Survival, Price = 40 },
            new Game { Id = 26, Name = "SCUM", Genre = Genre.Survival, Price = 30 },
            new Game { Id = 27, Name = "DayZ", Genre = Genre.Survival, Price = 45 },
            new Game { Id = 28, Name = "League of Legends", Genre = Genre.MOBA, Price = 0 },
            new Game { Id = 29, Name = "Dota 2", Genre = Genre.MOBA, Price = 0 },
            new Game { Id = 30, Name = "PUBG", Genre = Genre.BattleRoyale, Price = 20 }
        };
    }
}