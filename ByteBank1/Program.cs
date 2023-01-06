
using ByteBank.Entities;
using System.Globalization;
using System.Text.Json;

namespace ByteBank1
{
    public class Program
    {
        static void LerUsuarios(List<Usuario> usuarios, string fileName)
        {
            string jsonString = File.ReadAllText(fileName);

            if (!String.IsNullOrEmpty(jsonString))
            {
                List<Usuario> todosUsuarios = JsonSerializer.Deserialize<List<Usuario>>(jsonString);
                todosUsuarios.ForEach(usuario => usuarios.Add(usuario));
            }


        }
        static int OperaçãoDeNovaTentativa()
        {
            Console.Write("Deseja tentar novamente? (1 - Sim | 2 - Não) ");
            int opt = int.Parse(Console.ReadLine());
            while (opt != 1 && opt != 2)
            {
                Console.WriteLine("Opção invalida");
                Console.Write("Por favor digite novamente (1 - Sim | 2 - Não) ");
                opt = int.Parse(Console.ReadLine());
            }
            return opt;
        }
        static void SerializarJson(List<Usuario> usuarios, string fileName)
        {
            string jsonString = JsonSerializer.Serialize(usuarios);
            File.WriteAllText(fileName, jsonString);
        }
        static void MostrarMenu()
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
        static void MostrarSubMenu(Usuario usuario)
        {
            Console.WriteLine("<------------------>");
            Console.WriteLine($"| {usuario.Titular} | Saldo: {usuario.Saldo:F2} | Número da conta: {usuario.NumeroDaConta} |");
            Console.WriteLine("<------------------>");
            Console.WriteLine("1 - Depositar");
            Console.WriteLine("2 - Sacar");
            Console.WriteLine("3 - Trasferir");
            Console.WriteLine("4 - Alterar Senha");
            Console.WriteLine("5 - Sair");
            Console.WriteLine("<------------------>");
        }
        static void AlterarSenha(List<Usuario> usuarios, int userIndex, string fileName)
        {
            Console.Write("Por favor digite a sua senha atual: ");
            string senha = Console.ReadLine();
            Console.Write("Por favor digite a sua nova senha (minimo de 8 caracteres): ");
            string novaSenha = Console.ReadLine();
            usuarios[userIndex].Senha = novaSenha;
            SerializarJson(usuarios, fileName);
            Console.Clear();
            Console.WriteLine("Senha alterada com sucesso");
        }
        static void CadastrarNovoUsuario(List<Usuario> usuarios, string fileName)
        {
            Console.Write("Digite o cpf do usuario que deseja cadastrar: ");
            string cpf = Console.ReadLine();
            while (usuarios.Exists(usuario => usuario.Cpf.Equals(cpf)))
            {
                Console.WriteLine("Este Cpf ja esta sendo utilizado por outro usuario, por favor digite novamente");
                cpf = Console.ReadLine();
            }
            Console.Write("Digite o nome do usuario que deseja cadastrar: ");
            string titular = Console.ReadLine();
            Console.Write("Digite a senha que deseja cadastrar ( mínimo de 8 caracteres) : ");
            string senha = Console.ReadLine();

            while (String.IsNullOrEmpty(senha) || senha.Length < 8)
            {
                Console.WriteLine("A senha não possui 8 caracteres, por favor digite novamente");
                senha = Console.ReadLine();
            }

            Usuario novoUsuario = new Usuario(cpf, titular, senha);
            novoUsuario.GerarNumeroDaConta(usuarios);
            usuarios.Add(novoUsuario);

            SerializarJson(usuarios, fileName);
            Console.Clear();
            Console.WriteLine("<---------------------------------------------->");
            Console.WriteLine($"Usuario {novoUsuario.Titular} criado com sucesso!");
            Console.WriteLine("<---------------------------------------------->");
            MostrarUsuario(usuarios, true);


        }
        static void MostrarTodosUsuarios(List<Usuario> usuarios)
        {
            Console.Clear();
            Console.WriteLine("<--------------------------------------------------------------------------------->");
            usuarios.ForEach(usuario =>
            {
                string saldo = $"{usuario.Saldo:F2}";
                Console.WriteLine("| Titular: {0,-20}| CPF: {1,-5}| Saldo: R${2,-10}| Numero da conta: {3,-5}|", usuario.Titular, usuario.Cpf, saldo, usuario.NumeroDaConta);
            });
            Console.WriteLine("<--------------------------------------------------------------------------------->");
        }
        static void DeletarUsuario(List<Usuario> usuarios, string fileName)
        {
            Console.WriteLine("Digite o CPF do usuario que deseja deletar");
            string cpfInput = Console.ReadLine();


            if (usuarios.Exists(usuario => usuario.Cpf == cpfInput))
            {
                int userIndex = usuarios.FindIndex(usuario => usuario.Cpf == cpfInput);
                Console.Clear();
                Console.WriteLine("<---------------------------------------------->");
                Console.WriteLine($"Usuario {usuarios[userIndex].Titular} deletado com sucesso");
                Console.WriteLine("<---------------------------------------------->");
                usuarios.RemoveAt(userIndex);
                SerializarJson(usuarios, fileName);

            }
            else
            {
                Console.WriteLine("Usuario inexistente");
            }


        }
        static void MostrarUsuario(List<Usuario> usuarios, bool criação)
        {
            if (criação)
            {
                Console.WriteLine("<---------------------------------------------->");
                Console.WriteLine($" Titular: {usuarios[usuarios.Count - 1].Titular}");
                Console.WriteLine($" CPF: {usuarios[usuarios.Count - 1].Cpf}");
                Console.WriteLine($" Numero da conta: {usuarios[usuarios.Count - 1].NumeroDaConta}");
                Console.WriteLine("<---------------------------------------------->");
            }
            else
            {
                Console.WriteLine("Digite o CPF do usuario que deseja visualizar");
                string cpfInput = Console.ReadLine();

                if (usuarios.Exists(usuario => usuario.Cpf == cpfInput))
                {
                    Console.Clear();
                    int userIndex = usuarios.FindIndex(usuario => usuario.Cpf == cpfInput);
                    Console.WriteLine("<---------------------------------------------->");
                    Console.WriteLine($" Titular: {usuarios[userIndex].Titular}");
                    Console.WriteLine($" CPF: {usuarios[userIndex].Cpf}");
                    Console.WriteLine($" Numero da conta: {usuarios[userIndex].NumeroDaConta}");
                    Console.WriteLine("<---------------------------------------------->");
                }
                else
                {
                    Console.WriteLine("Usuario inexistente");
                }

            }
        }
        static void MostrarTotalnoBanco(List<Usuario> usuarios)
        {
            double soma = 0;
            usuarios.ForEach(usuario => soma += usuario.Saldo);
            Console.Clear();
            Console.WriteLine("<---------------------------------------------->");
            Console.WriteLine($"Saldo total do banco: R${soma:F2}");
            Console.WriteLine("<---------------------------------------------->");

        }
        static void Depositar(List<Usuario> usuarios, int userIndex, string fileName)
        {
            while (true)
            {
                Console.WriteLine("Digite o valor que deseja depositar na sua conta");
                double deposito = double.Parse(Console.ReadLine());
                if (deposito > 0)
                {
                    usuarios[userIndex].Saldo += deposito;
                    SerializarJson(usuarios, fileName);
                    Console.Clear();
                    Console.WriteLine("<---------------------------------------------->");
                    Console.WriteLine($"Deposito no valor de R${deposito:F2} realidado com sucesso! ");
                    Console.WriteLine("<---------------------------------------------->");
                    break;
                }
                else
                {
                    Console.WriteLine("valor invalido para realizar essa operação");
                    int opt = OperaçãoDeNovaTentativa();
                    if (opt == 2)
                    {
                        break;
                    }
                    else if (opt == 1)
                    {
                        continue;
                    }
                }
            }
        }
        static void Sacar(List<Usuario> usuarios, int userIndex, string fileName)
        {
            while (true)
            {
                Console.WriteLine("Digite o valor que deseja sacar de sua conta");
                double saque = double.Parse(Console.ReadLine());
                if (usuarios[userIndex].Saldo >= saque)
                {
                    usuarios[userIndex].Saldo -= saque;
                    SerializarJson(usuarios, fileName);
                    Console.Clear();
                    Console.WriteLine("<---------------------------------------------->");
                    Console.WriteLine($"Saque no valor de R${saque:F2} realidado com sucesso! ");
                    Console.WriteLine("<---------------------------------------------->");
                    break;
                }
                else
                {
                    Console.WriteLine("Saldo insuficiente para realizar essa operação");
                    int opt = OperaçãoDeNovaTentativa();
                    if (opt == 2)
                    {
                        break;
                    }
                    else if (opt == 1)
                    {
                        continue;
                    }
                }
            }
        }
        static void Transferir(List<Usuario> usuarios, int userIndex, string fileName)
        {
            while (true)
            {
                Console.Write("Digite o numero da conta para qual deseja transferir: ");
                int numeroDaContaDestinatario = int.Parse(Console.ReadLine());

                if (usuarios.Exists(usuario => usuario.NumeroDaConta == numeroDaContaDestinatario))
                {

                    Console.Write("Digite o valor que deseja tranferir: ");
                    double valor = double.Parse(Console.ReadLine());

                    if (usuarios[userIndex].Saldo >= valor)
                    {
                        int destinatarioIndex = usuarios.FindIndex(usuario => usuario.NumeroDaConta == numeroDaContaDestinatario);
                        usuarios[userIndex].Saldo -= valor;
                        usuarios[destinatarioIndex].Saldo += valor;
                        SerializarJson(usuarios, fileName);
                        Console.Clear();
                        Console.WriteLine("<---------------------------------------------->");
                        Console.WriteLine($"Tranferencia no valor de R${valor:F2} para {usuarios[destinatarioIndex].Titular} realizada com sucesso");
                        Console.WriteLine("<---------------------------------------------->");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Saldo insuficiente para realizar essa operação");
                        int opt = OperaçãoDeNovaTentativa();
                        if (opt == 2)
                        {
                            break;
                        }
                        else if (opt == 1)
                        {
                            continue;
                        }
                    }


                }
                else
                {
                    Console.WriteLine("Conta inexistente");
                    int opt = OperaçãoDeNovaTentativa();
                    if (opt == 2)
                    {
                        break;
                    }
                    else if (opt == 1)
                    {
                        continue;
                    }


                }

            }


        }
        static void FazerLogin(List<Usuario> usuarios, string fileName)
        {
            Console.WriteLine("<---------------------------------------------->");
            Console.WriteLine("Por favor realize o login em sua conta para continuar");
            while (true)
            {
                Console.Write("Digite o seu CPF: ");
                string cpfInput = Console.ReadLine();

                if (usuarios.Exists(usuario => usuario.Cpf == cpfInput))
                {
                    int userIndex = usuarios.FindIndex(usuario => usuario.Cpf == cpfInput);
                    Console.Write("Digite a sua senha: ");

                    if (Console.ReadLine() == usuarios[userIndex].Senha)
                    {
                        Console.Clear();
                        Console.WriteLine($"Seja bem vindo {usuarios[userIndex].Titular}");
                        int subOption;
                        do
                        {
                            MostrarSubMenu(usuarios[userIndex]);
                            subOption = int.Parse(Console.ReadLine());

                            switch (subOption)
                            {
                                case 1:
                                    Depositar(usuarios, userIndex, fileName);
                                    break;
                                case 2:
                                    Sacar(usuarios, userIndex, fileName);
                                    break;
                                case 3:
                                    Transferir(usuarios, userIndex, fileName);
                                    break;
                                case 4:
                                    AlterarSenha(usuarios, userIndex, fileName);
                                    break;
                                case 5:
                                    Console.Clear();
                                    Console.WriteLine("Obrigado por usar o ByteBank, volte sempre :)");
                                    break;
                                default:
                                    Console.WriteLine("Opção invalida, por favor digite opçoes entre 1, 2, 3, 4 ou 5");
                                    break;
                            }
                        } while (subOption != 5);
                        break;

                    }
                    else
                    {
                        Console.WriteLine("Senha invalida ");
                        int opt = OperaçãoDeNovaTentativa();
                        if (opt == 2)
                        {
                            break;
                        }
                        else if (opt == 1)
                        {
                            continue;
                        }
                    }

                }
                else
                {
                    Console.WriteLine("CPF não encontrado");
                    int opt = OperaçãoDeNovaTentativa();
                    if (opt == 2)
                    {
                        break;
                    }
                    else if (opt == 1)
                    {
                        continue;
                    }
                }
            }

        }
        public static void Main(string[] args)
        {
            Console.WriteLine("Seja bem vindo(a) ao Byte Bank!");
            List<Usuario> usuarios = new List<Usuario>();

            string fileName = "Usuarios.json";

            LerUsuarios(usuarios, fileName);

            int option;

            do
            {
                MostrarMenu();
                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 0:
                        Console.WriteLine("Estou encerrando o programa...");
                        break;
                    case 1:
                        CadastrarNovoUsuario(usuarios, fileName);
                        break;
                    case 2:
                        DeletarUsuario(usuarios, fileName);
                        break;
                    case 3:
                        MostrarTodosUsuarios(usuarios);
                        break;
                    case 4:
                        MostrarUsuario(usuarios, false);
                        break;
                    case 5:
                        MostrarTotalnoBanco(usuarios);
                        break;
                    case 6:
                        FazerLogin(usuarios, fileName);
                        break;
                    default:
                        Console.WriteLine("Opção invalida, por vafor digite opçoes entre 0, 1, 2, 3, 4, 5 ou 6");
                        break;
                }

            } while (option != 0);



        }

    }
}