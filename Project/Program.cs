using Project.Admin;
using Project.UserMenus;
using Project.Services.Interfaces;
using Project.Services.Implementations;
using Project.Data;
using Project.Enums;
using Project.Helpers;
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        LogoHelper.ShowLogo();

        var db = new DatabaseContext();

        IGameService gameService = new GameService(db);
        ICartService cartService = new CartService(gameService);
        IUserService userService = new UserService(db);
        IAuthService authService = new AuthService(userService);
        OrderService orderService = new OrderService(cartService, db);

        
        if (!userService.GetAll().Any(u => u.Role == UserRole.Admin))
        {
            userService.CreateAdmin("admin", "1234");
        }

        var adminMenu = new AdminMenu(gameService);
        var shopMenu = new ShopMenu(gameService, cartService);
        var cartMenu = new CartMenu(cartService, orderService, authService);
        var userMenu = new UserMenu(shopMenu, cartMenu);
        var mainMenu = new MainMenu(adminMenu, userMenu, authService);

        orderService.OnOrderCreated += order =>
        {
            Console.WriteLine($"\nOrder #{order.Id} created successfully!");
        };

        mainMenu.Show();
    }
}