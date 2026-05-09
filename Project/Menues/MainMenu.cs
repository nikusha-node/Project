using Project.Admin;
using Project.Helpers;
using Project.Services.Interfaces;
using Project.UserMenus;
using System;
using System.Data;
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
            Console.WriteLine("\n=== GAME STORE ===");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Admin");
            Console.WriteLine("4. User");
            Console.WriteLine("0. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var loginName = InputHelper.ReadString("Username: ");
                    var loginPassword = InputHelper.ReadString("Password: ");

                    var user = _authService.Login(loginName, loginPassword);

                    Console.WriteLine(user == null ? "Invalid credentials" : "Logged in!");
                    break;

                case "2":
                    var regName = InputHelper.ReadString("Username: ");
                    var regPassword = InputHelper.ReadString("Password: ");

                    _authService.Register(regName, regPassword);

                    Console.WriteLine("Registered and logged in!");
                    break;

                case "3":
                    var currentUser = _authService.GetCurrentUser();

                    if (currentUser == null)
                    {
                        Console.WriteLine("You must login first!");
                        break;
                    }

                    if (currentUser.Role != UserRole.Admin)
                    {
                        Console.WriteLine("Access denied! Admin only.");
                        break;
                    }

                    _adminMenu.Show();
                    break;

                case "4":
                    if (_authService.GetCurrentUser() == null)
                    {
                        Console.WriteLine("You must login first!");
                        break;
                    }

                    _userMenu.Show();
                    break;
            }
        }
    }
}
