using Project.UserMenus;

namespace Project.UserMenus;

public class UserMenu
{
    private readonly ShopMenu _shopMenu;
    private readonly CartMenu _cartMenu;

    public UserMenu(ShopMenu shopMenu, CartMenu cartMenu)
    {
        _shopMenu = shopMenu;
        _cartMenu = cartMenu;
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine("\n=== USER MENU ===");
            Console.WriteLine("1. Shop");
            Console.WriteLine("2. Cart");
            Console.WriteLine("0. Back");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _shopMenu.Show();
                    break;
                case "2":
                    _cartMenu.Show();
                    break;
                case "0":
                    return;
            }
        }
    }
}