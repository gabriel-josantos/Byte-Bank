using ByteBank.Helpers;
using ByteBank.Model.DTO;
using ByteBank.Model.Entities;
using ByteBank.Service.Exceptions;
using ByteBank.View;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ByteBank.Service;

public class AccountService
{
    public readonly string Agency;
    private List<Account> _accounts;
    private Account? _loggedAccount;
    private bool _isAccountLogged { get { return _loggedAccount != null; } }

    public AccountService(string agency)
    {
        _accounts = new List<Account>();
        _loggedAccount = null;
        Agency = agency;
    }
    public void DesserializeJson(string fileName)
    {
        string jsonString = File.ReadAllText(fileName);

        if (!string.IsNullOrEmpty(jsonString))
        {
          _accounts = JsonSerializer.Deserialize<List<Account>>(jsonString);

        }

    }
    public void SerializeJson(string fileName)
    {
        string jsonString = JsonSerializer.Serialize(_accounts);
        File.WriteAllText(fileName, jsonString);
    }
    public long GerarNumeroDaConta(List<Account> usuarios)
    {
        Random rnd = new Random();
        long numeroGerado = rnd.Next(100, 1000);

        while (usuarios.Exists(usuario => usuario.AccountNumber == numeroGerado))
        {
            numeroGerado = rnd.Next(100, 1000);
        }

        return numeroGerado;
    }
    public void Logout()
    {
        _loggedAccount = null;
    }

    public void Login(LoginFormDto loginForm)
    {
        if (!DoesAccountExists(loginForm.Cpf))
            return;
        if (!DoesPasswordsMatch(loginForm))
            throw new PasswordDoesNotMatchException("A senha está incorreta");

        Account account = FindAccount(loginForm.Cpf);

        if (account.IsBlocked)
            throw new AccountIsBlockedException("A conta não está autorizada");

        _loggedAccount = account;
    }

    public void CreateUser(string fileName)
    {
        Console.Write("Digite o cpf do usuario que deseja cadastrar: ");
        string cpf = Console.ReadLine();
        while (_accounts.Exists(conta => conta.Cpf.Equals(cpf)))
        {
            Console.WriteLine("Este Cpf ja esta sendo utilizado por outro usuario, por favor digite novamente");
            cpf = Console.ReadLine();
        }
        Console.Write("Digite o nome do usuario que deseja cadastrar: ");
        string titular = Console.ReadLine();
        Console.Write("Digite a senha que deseja cadastrar ( mínimo de 8 caracteres) : ");
        string senha = Console.ReadLine();

        while (string.IsNullOrEmpty(senha) || senha.Length < 8)
        {
            Console.WriteLine("A senha não possui 8 caracteres, por favor digite novamente");
            senha = Console.ReadLine();
        }

        Account novaConta = new Account(cpf, titular, senha, GerarNumeroDaConta(_accounts));
        _accounts.Add(novaConta);

        SerializeJson(fileName);

        Console.Clear();
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($"Usuario {novaConta.Name} criado com sucesso!");
        Console.WriteLine("<---------------------------------------------->");
        ShowUser(true);

    }

    public void ShowAllUsers()
    {
        Console.Clear();
        Console.WriteLine("<--------------------------------------------------------------------------------->");
        _accounts.ForEach(account =>
        {
            string saldo = $"{account.Balance:F2}";
            Console.WriteLine("| Titular: {0,-20}| CPF: {1,-5}| Saldo: R${2,-10}| Numero da conta: {3,-5}|", account.Name, account.Cpf, saldo, account.AccountNumber);
        });
        Console.WriteLine("<--------------------------------------------------------------------------------->");
    }

    public void DeleteUser(string fileName)
    {
        Console.WriteLine("Digite o CPF do usuario que deseja deletar");
        string cpfInput = Console.ReadLine();


        if (DoesAccountExists(cpfInput))
        {
            Account conta = FindAccount(cpfInput);
            Console.Clear();
            Console.WriteLine("<---------------------------------------------->");
            Console.WriteLine($"Usuario {conta.Name} deletado com sucesso");
            Console.WriteLine("<---------------------------------------------->");
            _accounts.Remove(conta);
            SerializeJson(fileName);

        }
        else
        {
            Console.WriteLine("Usuario inexistente");
        }


    }

    public void ShowUser(bool criação)
    {
        if (criação)
        {
            Console.WriteLine("<---------------------------------------------->");
            Console.WriteLine($" Titular: {_accounts[_accounts.Count - 1].Name}");
            Console.WriteLine($" CPF: {_accounts[_accounts.Count - 1].Cpf}");
            Console.WriteLine($" Numero da conta: {_accounts[_accounts.Count - 1].AccountNumber}");
            Console.WriteLine("<---------------------------------------------->");
        }
        else
        {
            Console.WriteLine("Digite o CPF do usuario que deseja visualizar");
            string cpfInput = Console.ReadLine();

            if (DoesAccountExists(cpfInput))
            {
                Console.Clear();
                Account account = FindAccount(cpfInput);
                Console.WriteLine("<---------------------------------------------->");
                Console.WriteLine($" Titular: {account.Name}");
                Console.WriteLine($" CPF: {account.Cpf}");
                Console.WriteLine($" Numero da conta: {account.AccountNumber}");
                Console.WriteLine($" Bloqueada: {account.IsBlocked}");
                Console.WriteLine("<---------------------------------------------->");
            }
            else
            {
                Console.WriteLine("Usuario inexistente");
            }

        }
    }

    public void ShowBankBalance()
    {
        double sum = 0;
        _accounts.ForEach(account => sum += account.Balance);
        Console.Clear();
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($"Saldo total do banco: R${sum:F2}");
        Console.WriteLine("<---------------------------------------------->");

    }

    private bool DoesPasswordsMatch(LoginFormDto loginForm)
    {
        Account account = _accounts.Find(account => account.Cpf == loginForm.Cpf);

        return loginForm.Password.Equals(account.Password);
    }

    private bool DoesAccountExists(string cpf)
    {
        return _accounts.Exists(account => account.Cpf == cpf);
    }

    private Account FindAccount(string cpf)
    {
        return _accounts.Find(conta => conta.Cpf == cpf);

    }

    public void Deposit()
    {

        Console.WriteLine("Digite o valor que deseja depositar na sua conta");
        double depositAmount = double.Parse(Console.ReadLine());

        if (depositAmount <= 0)
            throw new InvalidAmountException("Valor invalido");

        _loggedAccount.Deposit(depositAmount);

        Console.Clear();
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($"Deposito no valor de R${depositAmount:F2} realidado com sucesso! ");
        Console.WriteLine("<---------------------------------------------->");

    }

    public void Withdraw()
    {

        Console.WriteLine("Digite o valor que deseja sacar de sua conta");
        double withdrawalAmount = double.Parse(Console.ReadLine());

        if (!HasSufficientBalance(withdrawalAmount))
            throw new InsuficientBalanceException();

        _loggedAccount.Withdraw(withdrawalAmount);
        Console.Clear();
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($"Deposito no valor de R${withdrawalAmount:F2} realidado com sucesso! ");
        Console.WriteLine("<---------------------------------------------->");



    }

    public void Transfer()
    {

        Console.Write("Digite o cpf da conta que deseja tranferir: ");
        string cpf = Console.ReadLine();

        if (!DoesAccountExists(cpf))
            throw new AccountDoesNotExistsException("Essa conta nao existe");


        Console.Write("Digite o valor que deseja tranferir: ");
        double transferAmount = double.Parse(Console.ReadLine());


        if (!HasSufficientBalance(transferAmount))
            throw new InsuficientBalanceException();


        Account destinationAccount = FindAccount(cpf);

        _loggedAccount.Transfer(transferAmount, destinationAccount);

        Console.Clear();
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($"Tranferencia no valor de R${transferAmount:F2} para {destinationAccount.Name} realizada com sucesso");
        Console.WriteLine("<---------------------------------------------->");


    }

    public void ChangePassword()
    {
        Console.Write("Por favor digite a sua senha atual: ");
        string currentPassword = Console.ReadLine();
        Console.Write("Por favor digite a sua nova senha (minimo de 8 caracteres): ");
        string newPassword = Console.ReadLine();
        // contaAtual.Senha = novaSenha;
        Console.Clear();
        Console.WriteLine("Senha alterada com sucesso");
    }


    private bool HasSufficientBalance(double amount)
    {
        return amount <= _loggedAccount.Balance;
    }


    public void ShowUserAccountOptions()
    {
        int accOption;
        do
        {
            MenuView.ShowUserMenu(_loggedAccount);

            accOption = int.Parse(Console.ReadLine());

            switch (accOption)
            {
                case 1:
                    Deposit();
                    break;
                case 2:
                    Withdraw();
                    break;
                case 3:
                    Transfer();
                    break;
                case 4:
                    ChangePassword();
                    break;
                case 5:
                    Console.Clear();
                    Logout();
                    Console.WriteLine("Obrigado por usar o ByteBank, volte sempre :)");
                    break;
                default:
                    Console.WriteLine("Opção invalida, por favor digite opçoes entre 1, 2, 3, 4 ou 5");
                    break;
            }
        }
        while (accOption != 5);
    }

    public void ShowMainMenuOptions(int option, string fileName)
    {

        try
        {
            switch (option)
            {
                case 0:
                    Console.WriteLine("Estou encerrando o programa...");
                    break;
                case 1:
                    CreateUser(fileName);
                    break;
                case 2:
                    DeleteUser(fileName);
                    break;
                case 3:
                    ShowAllUsers();
                    break;
                case 4:
                    ShowUser(false);
                    break;
                case 5:
                    ShowBankBalance();
                    break;
                case 6:
                    var loginForm = AccountView.MenuLogin();
                    Login(loginForm);
                    ShowUserAccountOptions();
                    break;
                default:
                    Console.WriteLine("Opção invalida, por vafor digite opçoes entre 0, 1, 2, 3, 4, 5 ou 6");
                    break;
            }
        }
        catch (InsuficientBalanceException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


}





