using ByteBank.Model.Entities;
using ByteBank.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.View
{
    public class MenuView
    {

        public static void ShowInitialMenu()
        {
            Console.WriteLine("<---------------------------------------------->");
            Console.WriteLine("Seja bem vindo ao ByteBank");
            Console.WriteLine("<---------------------------------------------->");
            Console.WriteLine("0 - Sair do programa");
            Console.WriteLine("1 - Fazer login (admin)");
            Console.WriteLine("2 - Fazer login (usuario)");
            Console.WriteLine("<---------------------------------------------->");
            Console.Write("Digite a opção desejada: ");
        }
        public static void ShowAdminMenu()
        {
            Console.WriteLine("<---------------------------------------------->");
            Console.WriteLine("1 - Registrar novo usuário");
            Console.WriteLine("2 - Deletar um usuário");
            Console.WriteLine("3 - Mostrar todas as contas cadastradas");
            Console.WriteLine("4 - Detalhes de um usuário");
            Console.WriteLine("5 - Logout");
            Console.WriteLine("<---------------------------------------------->");
            Console.Write("Digite a opção desejada: ");
        }
        public static void ShowUserMenu(Account conta)
        {
            Console.WriteLine("<------------------>");
            Console.WriteLine($"| {conta.Name} | Saldo: {conta.Balance:F2} | Número da conta: {conta.AccountNumber} |");
            Console.WriteLine("<------------------>");
            Console.WriteLine("1 - Depositar");
            Console.WriteLine("2 - Sacar");
            Console.WriteLine("3 - Trasferir");
            Console.WriteLine("4 - Alterar Senha");
            Console.WriteLine("5 - Logout");
            Console.WriteLine("<------------------>");
        }

    }
}
