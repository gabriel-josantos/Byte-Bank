
using ByteBank.Model.DTO;
using ByteBank.Model.Entities;
using ByteBank.Service.Exceptions;
using ByteBank.View;
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
    public void DesserializeJson(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);

        if (!string.IsNullOrEmpty(jsonString))
        {
            _accounts = JsonSerializer.Deserialize<List<Account>>(jsonString);

        }

    }
    public void SerializeJson(string filePath)
    {
        string jsonString = JsonSerializer.Serialize(_accounts);
        File.WriteAllText(filePath, jsonString);
    }
    public long GenerateAccountNumber(List<Account> accounts)
    {
        Random rnd = new Random();
        long generatedNumber = rnd.Next(100, 1000);

        while (accounts.Exists(account => account.AccountNumber == generatedNumber))
        {
            generatedNumber = rnd.Next(100, 1000);
        }

        return generatedNumber;
    }
    public void Logout()
    {
        _loggedAccount = null;
    }

    public bool IsUserAdmin(Account account)
    {
        return account.Permission.Equals("admin");
    }

    public void Login(LoginFormDto loginForm)
    {
        if (!DoesAccountExists(loginForm.Cpf))
            throw new AccountDoesNotExistsException("Essa conta nao exite");
        if (!DoesPasswordsMatch(loginForm))
            throw new UserNotAuthorizedException("A senha está incorreta");

        Account account = FindAccount(loginForm.Cpf);

        if (account.IsBlocked)
            throw new UserNotAuthorizedException("A conta está bloqueada");

        _loggedAccount = account;
    }
    public void Login(LoginFormDto loginForm, string admin)
    {
        if (!DoesAccountExists(loginForm.Cpf))
            throw new AccountDoesNotExistsException("Essa conta nao exite");
        if (!DoesPasswordsMatch(loginForm))
            throw new UserNotAuthorizedException("A senha está incorreta");

        Account account = FindAccount(loginForm.Cpf);

        if (account.IsBlocked)
            throw new UserNotAuthorizedException("A conta está bloqueada");

        if (!IsUserAdmin(account))
            throw new UserNotAuthorizedException("Este usuario nao possui permissao de administrador");


        _loggedAccount = account;
    }

    public void CreateUser(string filedPath)
    {
        Console.Write("Digite o cpf do usuario que deseja cadastrar: ");
        string cpf = Console.ReadLine();

        while (DoesAccountExists(cpf))
        {
            Console.WriteLine("Este Cpf ja esta sendo utilizado por outro usuario, por favor digite novamente");
            cpf = Console.ReadLine();
        }
        Console.Write("Digite o nome do usuario que deseja cadastrar: ");
        string name = Console.ReadLine();
        Console.Write("Digite a senha que deseja cadastrar ( mínimo de 8 caracteres) : ");
        string password = Console.ReadLine();

        while (string.IsNullOrEmpty(password) || password.Length < 8)
        {
            Console.WriteLine("A senha não possui 8 caracteres, por favor digite novamente");
            password = Console.ReadLine();
        }

        Account newAccount = new Account(cpf, name, password, GenerateAccountNumber(_accounts));
        _accounts.Add(newAccount);

        SerializeJson(filedPath);

        Console.Clear();
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($"Usuario {newAccount.Name} criado com sucesso!");
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($" Titular: {newAccount.Name}");
        Console.WriteLine($" CPF: {newAccount.Cpf}");
        Console.WriteLine($" Numero da conta: {newAccount.AccountNumber}");
        Console.WriteLine("<---------------------------------------------->");


    }

    public void ShowAllUsers()
    {
        Console.Clear();
        Console.WriteLine("<--------------------------------------------------------------------------------->");
        _accounts.ForEach(account =>
        {
            string balance = $"{account.Balance:F2}";
            Console.WriteLine("| Titular: {0,-20}| CPF: {1,-5}| Saldo: R${2,-10}| Numero da conta: {3,-5}|", account.Name, account.Cpf, balance, account.AccountNumber);
        });
        Console.WriteLine("<--------------------------------------------------------------------------------->");
    }

    public void BlockAccount(string filePath)
    {
        Console.Write("Digite o CPF do usuario da conta que deseja bloquear: ");
        string cpfInput = Console.ReadLine();

        if (!DoesAccountExists(cpfInput))
            throw new AccountDoesNotExistsException("Essa conta nao existe");

        Account account = FindAccount(cpfInput);
        account.BlockAccount();

        Console.WriteLine($"A conta de {account.Name} bloqueada com sucesso");

        SerializeJson(filePath);

    }

    public void UnblockAccount(string filePath)
    {
        Console.Write("Digite o CPF do usuario da conta que deseja desbloquear: ");
        string cpfInput = Console.ReadLine();

        if (!DoesAccountExists(cpfInput))
            throw new AccountDoesNotExistsException("Essa conta nao existe");

        Account account = FindAccount(cpfInput);
        account.UnblockAccount();

        Console.WriteLine($"A conta de {account.Name} desbloqueada com sucesso");

        SerializeJson(filePath);

    }

    public void DeleteUser(string filePath)
    {
        Console.Write("Digite o CPF do usuario que deseja deletar: ");
        string cpfInput = Console.ReadLine();


        if (!DoesAccountExists(cpfInput))
            throw new AccountDoesNotExistsException("essa conta nao existe");

        Account account = FindAccount(cpfInput);
        Console.Clear();
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($"Usuario {account.Name} deletado com sucesso");
        Console.WriteLine("<---------------------------------------------->");
        _accounts.Remove(account);
        SerializeJson(filePath);


    }

    public void ShowUserInfo()
    {
        Console.Write("Digite o CPF do usuario que deseja visualizar: ");
        string cpfInput = Console.ReadLine();

        if (!DoesAccountExists(cpfInput))
            throw new AccountDoesNotExistsException("Essa conta nao existe");

        Console.Clear();
        Account account = FindAccount(cpfInput);
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($" Titular: {account.Name}");
        Console.WriteLine($" CPF: {account.Cpf}");
        Console.WriteLine($" Numero da conta: {account.AccountNumber}");
        Console.WriteLine($" Bloqueada: {account.IsBlocked}");
        Console.WriteLine("<---------------------------------------------->");

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

    public void Deposit(string filePath)
    {

        Console.WriteLine("Digite o valor que deseja depositar na sua conta");
        double depositAmount = double.Parse(Console.ReadLine());

        if (depositAmount <= 0)
            throw new InvalidAmountException("Valor invalido");

        _loggedAccount.Deposit(depositAmount);

        SerializeJson(filePath);

        Console.Clear();
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($"Deposito no valor de R${depositAmount:F2} realidado com sucesso! ");
        Console.WriteLine("<---------------------------------------------->");

    }

    public void Withdraw(string filePath)
    {

        Console.WriteLine("Digite o valor que deseja sacar de sua conta");
        double withdrawalAmount = double.Parse(Console.ReadLine());

        if (!HasSufficientBalance(withdrawalAmount))
            throw new InsuficientBalanceException();

        _loggedAccount.Withdraw(withdrawalAmount);

        SerializeJson(filePath);
        Console.Clear();
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($"Saque no valor de R${withdrawalAmount:F2} realidado com sucesso! ");
        Console.WriteLine("<---------------------------------------------->");



    }

    public void Transfer(string filePath)
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

        SerializeJson(filePath);

        Console.Clear();
        Console.WriteLine("<---------------------------------------------->");
        Console.WriteLine($"Tranferencia no valor de R${transferAmount:F2} para {destinationAccount.Name} realizada com sucesso");
        Console.WriteLine("<---------------------------------------------->");


    }

    public void ChangePassword(string filePath)
    {
        Console.Write("Por favor digite a sua senha atual: ");
        string currentPassword = Console.ReadLine();
        Console.Write("Por favor digite a sua nova senha (minimo de 8 caracteres): ");
        string newPassword = Console.ReadLine();
        // contaAtual.Senha = novaSenha;
        Console.Clear();
        SerializeJson(filePath);
        Console.WriteLine("Senha alterada com sucesso");
    }


    private bool HasSufficientBalance(double amount)
    {
        return amount <= _loggedAccount.Balance;
    }


    public void UserAccountOptions(string filePath)
    {
        int option;
        do
        {
            MenuView.ShowUserMenu(_loggedAccount);
            option = int.Parse(Console.ReadLine());
            try
            {
                switch (option)
                {
                    case 1:
                        Deposit(filePath);
                        break;
                    case 2:
                        Withdraw(filePath);
                        break;
                    case 3:
                        Transfer(filePath);
                        break;
                    case 4:
                        ChangePassword(filePath);
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
            catch (InsuficientBalanceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidAmountException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (AccountDoesNotExistsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (UserNotAuthorizedException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        while (option != 5);
    }

    public void AdminAccoutOptions(string filePath)
    {
        int option;
        do
        {
            MenuView.ShowAdminMenu();
            option = int.Parse(Console.ReadLine());
            try
            {
                switch (option)
                {

                    case 1:
                        CreateUser(filePath);
                        break;
                    case 2:
                        DeleteUser(filePath);
                        break;
                    case 3:
                        ShowAllUsers();
                        break;
                    case 4:
                        ShowUserInfo();
                        break;
                    case 5:
                        ShowBankBalance();
                        break;
                    case 6:
                        BlockAccount(filePath);
                        break;
                    case 7:
                        UnblockAccount(filePath);
                        break;
                    case 8:
                        Console.Clear();
                        Logout();
                        Console.WriteLine("Obrigado por usar o ByteBank, volte sempre :)");
                        break;
                    default:
                        Console.WriteLine("Opção invalida, por vafor digite opçoes entre 0, 1, 2, 3, 4, 5 ou 6");
                        break;
                }
            }
            catch (AccountDoesNotExistsException ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (option != 8);
    }


}





