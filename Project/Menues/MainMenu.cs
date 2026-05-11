using Project.Admin;
using Project.Helpers;
using Project.Services.Interfaces;
using Project.UserMenus;
using Project.Enums;

namespace Project.UserMenus;

public class MainMenu
{
    private readonly AdminMenu _adminMenu;
    private readonly UserMenu _userMenu;
    private readonly IAuthService _authService;

    public MainMenu(AdminMenu adminMenu, UserMenu userMenu, IAuthService authService)
    {
        _adminMenu = adminMenu;
        _userMenu = userMenu;
        _authService = authService;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            LogoHelper.ShowLogo();
            UIHelper.Divider();
            UIHelper.WriteLineCentered("🎮  GAME STORE MENU  🎮", ConsoleColor.Yellow);
            UIHelper.Divider();
            UIHelper.WriteLineCentered("1.  🔐  Login", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("2.  📝  Register", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("3.  🛡️   Admin Panel", ConsoleColor.Magenta);
            UIHelper.WriteLineCentered("4.  🛒  User Shop", ConsoleColor.Cyan);
            UIHelper.WriteLineCentered("0.  🚪  Exit", ConsoleColor.DarkGray);
            UIHelper.Divider();
            int screenWidth = Console.WindowWidth;
            string prompt = "Enter your choice: ";
            int padding = Math.Max(0, (screenWidth - prompt.Length) / 2);
            Console.Write(new string(' ', padding));
            UIHelper.SetColor(ConsoleColor.White);
            var choice = Console.ReadLine();
            UIHelper.ResetColor();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    LogoHelper.ShowLogo();
                    UIHelper.WriteLineCentered("=== 🔐 LOGIN ===", ConsoleColor.Yellow);
                    UIHelper.Divider();
                    var loginName = InputHelper.ReadString("Username: ");
                    var loginPassword = InputHelper.ReadString("Password: ");
                    var user = _authService.Login(loginName, loginPassword);
                    if (user == null) UIHelper.Error("Invalid credentials!");
                    else UIHelper.Success("Welcome back! Logged in!");
                    Console.ReadKey();
                    break;

                case "2":
                    Console.Clear();
                    LogoHelper.ShowLogo();
                    UIHelper.WriteLineCentered("=== 📝 REGISTER ===", ConsoleColor.Yellow);
                    UIHelper.Divider();
                    var regName = InputHelper.ReadString("Username: ");
                    var regPassword = InputHelper.ReadString("Password: ");
                    _authService.Register(regName, regPassword);
                    UIHelper.Success("Account created! You're logged in!");
                    Console.ReadKey();
                    break;

                case "3":
                    var currentUser = _authService.GetCurrentUser();
                    if (currentUser == null)
                    {
                        UIHelper.WriteLineCentered("You must login first!", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                    }
                    if (currentUser.Role != UserRole.Admin)
                    {
                        UIHelper.WriteLineCentered("Access denied! Admin only.", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                    }
                    _adminMenu.Show();
                    break;

                case "4":
                    if (_authService.GetCurrentUser() == null)
                    {
                        UIHelper.WriteLineCentered("You must login first!", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                    }
                    _userMenu.Show();
                    break;

                case "0":
                    UIHelper.WriteLineCentered("Goodbye!", ConsoleColor.Green);
                    Console.ReadKey();
                    return;

                default:
                    UIHelper.WriteLineCentered("Invalid choice!", ConsoleColor.Red);
                    Console.ReadKey();
                    break;
            }
        }
    }
}