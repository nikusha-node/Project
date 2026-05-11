using Project.Helpers;

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
            Console.Clear();
            LogoHelper.ShowLogo();

            UIHelper.Divider();
            UIHelper.WriteLineCentered("🎮  USER MENU  🎮", ConsoleColor.Yellow);
            UIHelper.Divider();
            UIHelper.WriteLineCentered("1.  🛒  Shop", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("2.  🧾  Cart", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("0.  🚪  Back", ConsoleColor.DarkGray);
            UIHelper.Divider();

            var choice = UIHelper.ReadLineCentered("Enter your choice: ", ConsoleColor.Cyan);

            switch (choice)
            {
                case "1": _shopMenu.Show(); break;
                case "2": _cartMenu.Show(); break;
                case "0": return;
                default:
                    UIHelper.Error("Invalid choice!");
                    Console.ReadKey();
                    break;
            }
        }
    }
}