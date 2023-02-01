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
        public static void ShowMainMenu()
        {
            Console.WriteLine("<---------------------------------------------->");
            Console.WriteLine("1 - Registrar novo usuário");
            Console.WriteLine("2 - Deletar um usuário");
            Console.WriteLine("3 - Mostrar todas as contas cadastradas");
            Console.WriteLine("4 - Detalhes de um usuário");
            Console.WriteLine("5 - Total armazenado no banco");
            Console.WriteLine("6 - Fazer login na sua conta");
            Console.WriteLine("0 - Para sair do programa");
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
            Console.WriteLine("5 - Sair");
            Console.WriteLine("<------------------>");
        }

    }
}
