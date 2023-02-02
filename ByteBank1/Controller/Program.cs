using ByteBank.Helpers;
using ByteBank.Model.Entities;
using ByteBank.Service;
using ByteBank.Service.Exceptions;
using ByteBank.View;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.Text.Json;

namespace ByteBank.Controller
{
    public class Program
    {

        public static void Main(string[] args)
        {
            AccountService accountService = new AccountService("1288");
            string filePath = @"..\..\..\..\data\Contas.json";

            accountService.DesserializeJson(filePath);

            int option;

            do
            {
                MenuView.ShowInitialMenu();

                option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        var loginFormAdmin = AccountView.MenuLogin();
                        accountService.Login(loginFormAdmin, "admin");
                        accountService.AdminAccoutOptions(filePath);
                        break;
                    case 2:
                        var loginFormUser = AccountView.MenuLogin();
                        accountService.Login(loginFormUser);
                        accountService.UserAccountOptions(filePath);
                        break;
                    default:
                        break;

                }

            } while (option != 0);



        }

    }
}