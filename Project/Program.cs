using Project.Admin;
using Project.UserMenus;
using Project.Services.Interfaces;
using Project.Services.Implementations;
using Project.Data;

class Program
{
    static void Main()
    {
        
        var db = new DatabaseContext();

        
        IGameService gameService = new GameService(db);
        ICartService cartService = new CartService(gameService);
        IUserService userService = new UserService(db);
        IAuthService authService = new AuthService(userService);
        IOrderService orderService = new OrderService(cartService, db);

        
        var adminMenu = new AdminMenu(gameService);
        var shopMenu = new ShopMenu(gameService, cartService);
        var cartMenu = new CartMenu(cartService, orderService, authService);
        var userMenu = new UserMenu(shopMenu, cartMenu);
        var mainMenu = new MainMenu(adminMenu, userMenu, authService);

        mainMenu.Show();



        if (!userService.GetAll().Any(u => u.Role == Project.Enums.UserRole.Admin))
        {
            userService.CreateAdmin("admin");
        }
    }
}