using Project.Services.Interfaces;
using Project.Services.Implementations;
using Project.UserMenus;
using Project.Admin;



class Program
{
    static void Main()
    {
        IGameService gameService = new GameService();
        ICartService cartService = new CartService(gameService);
        IOrderService orderService = new OrderService(cartService);
        IUserService userService = new UserService();
        IAuthService authService = new AuthService(userService);

        var adminMenu = new AdminMenu(gameService);
        var shopMenu = new ShopMenu(gameService, cartService);
        var cartMenu = new CartMenu(cartService, orderService, authService);
        var userMenu = new UserMenu(shopMenu, cartMenu);
        var mainMenu = new MainMenu(adminMenu, userMenu, authService);

        mainMenu.Show();
    }
}