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
            string fileName = "Contas.json";

            accountService.DesserializeJson(fileName);

            int option;

            do
            {
                MenuView.ShowMainMenu();
                option = int.Parse(Console.ReadLine());

                accountService.ShowMainMenuOptions(option, fileName);


            } while (option != 0);



        }

    }
}