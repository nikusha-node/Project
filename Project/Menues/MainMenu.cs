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

            var currentUser = _authService.GetCurrentUser();
            if (currentUser != null)
            {
                var userColor = currentUser.Role == UserRole.Admin
                    ? ConsoleColor.Magenta
                    : ConsoleColor.Green;
                UIHelper.WriteLineCentered(
                    $"👤  Logged in as: {currentUser.Username}  [{currentUser.Role}]",
                    userColor
                );
            }
            else
            {
                UIHelper.WriteLineCentered("👤  Not logged in", ConsoleColor.DarkGray);
            }

            UIHelper.Divider();
            UIHelper.WriteLineCentered("🎮  GAME STORE MENU  🎮", ConsoleColor.Yellow);
            UIHelper.Divider();

            if (currentUser == null)
            {
                UIHelper.WriteLineCentered("1.  🔐  Login", ConsoleColor.Cyan);
                UIHelper.WriteLineCentered("2.  📝  Register", ConsoleColor.Cyan);
            }
            else
            {
                UIHelper.WriteLineCentered("1.  🔐  Switch Account", ConsoleColor.DarkCyan);
                UIHelper.WriteLineCentered("2.  📝  Register", ConsoleColor.DarkCyan);
            }

            if (currentUser?.Role == UserRole.Admin)
                UIHelper.WriteLineCentered("3.  🛡️   Admin Panel", ConsoleColor.Magenta);
            else
                UIHelper.WriteLineCentered("3.  🛡️   Admin Panel", ConsoleColor.DarkGray);
            if (currentUser != null)
                UIHelper.WriteLineCentered("4.  🛒  User Shop", ConsoleColor.Green);
            else
                UIHelper.WriteLineCentered("4.  🛒  User Shop", ConsoleColor.DarkGray);
            if (currentUser != null)
                UIHelper.WriteLineCentered("5.  🚪  Logout", ConsoleColor.DarkYellow);

            UIHelper.Divider();
            UIHelper.WriteLineCentered("0.  🚪  Exit", ConsoleColor.Red);
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
                    var loginPassword = InputHelper.ReadPassword("Password: "); 
                    var user = _authService.Login(loginName, loginPassword);
                    if (user == null)
                        UIHelper.Error("Invalid credentials!");
                    else
                    {
                        var welcomeColor = user.Role == UserRole.Admin
                            ? ConsoleColor.Magenta
                            : ConsoleColor.Green;
                        UIHelper.WriteLineCentered($"✅  Welcome back, {user.Username}!", welcomeColor);
                    }
                    Console.ReadKey();
                    break;

                case "2":
                    Console.Clear();
                    LogoHelper.ShowLogo();
                    UIHelper.WriteLineCentered("=== 📝 REGISTER ===", ConsoleColor.Yellow);
                    UIHelper.Divider();
                    var regName = InputHelper.ReadString("Username: ");
                    var regPassword = InputHelper.ReadPassword("Password: "); 
                    try
                    {
                        _authService.Register(regName, regPassword);
                        UIHelper.Success("Account created! You're logged in!");
                    }
                    catch (Exception ex)
                    {
                        UIHelper.Error(ex.Message); 
                    }
                    Console.ReadKey();
                    break;

                case "3":
                    if (currentUser == null)
                    {
                        UIHelper.Error("You must login first!");
                        Console.ReadKey();
                        break;
                    }
                    if (currentUser.Role != UserRole.Admin)
                    {
                        UIHelper.Error("Access denied! Admin only.");
                        Console.ReadKey();
                        break;
                    }
                    _adminMenu.Show();
                    break;

                case "4":
                    if (currentUser == null)
                    {
                        UIHelper.Error("You must login first!");
                        Console.ReadKey();
                        break;
                    }
                    _userMenu.Show();
                    break;

                case "5":
                    if (currentUser == null)
                    {
                        UIHelper.Error("You are not logged in!");
                        Console.ReadKey();
                        break;
                    }
                    _authService.Logout();
                    UIHelper.Success($"👋  Logged out successfully!");
                    Console.ReadKey();
                    break;

                case "0":
                    UIHelper.WriteLineCentered("👋  Goodbye! See you next time!", ConsoleColor.Green);
                    Console.ReadKey();
                    return;

                default:
                    UIHelper.Error("Invalid choice!");
                    Console.ReadKey();
                    break;
            }
        }
    }
}