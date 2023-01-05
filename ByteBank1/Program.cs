
using ByteBank.Entities;
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
        static void SerializarJson(List<Usuario> usuarios, string fileName)
        {
            string jsonString = JsonSerializer.Serialize(usuarios);
            File.WriteAllText(fileName, jsonString);
        }
        static void MostrarMenu()
        {
            Console.WriteLine("<------------------------------------------->");
            Console.WriteLine("1 - Registrar novo usuário");
            Console.WriteLine("2 - Deletar um usuário");
            Console.WriteLine("3 - Mostrar todas as contas registradas");
            Console.WriteLine("4 - Detalhes de um usuário");
            Console.WriteLine("5 - Total armazenado no banco");
            Console.WriteLine("6 - Manipular a conta");
            Console.WriteLine("0 - Para sair do programa");
            Console.WriteLine("<-------------------------------------------->");
            Console.Write("Digite a opção desejada: ");
      
        }
        static void MostrarSubMenu()
        {
            Console.WriteLine("<------------------>");
            Console.WriteLine("1 - Depositar");
            Console.WriteLine("2 - Sacar");
            Console.WriteLine("3 - Trasferir");
            Console.WriteLine("4 - Sair");
            Console.WriteLine("<------------------>");
        }
        static void RegistrarNovoUsuario(List<Usuario> usuarios, string fileName)
        {
            Console.Write("Digite o cpf: ");
            string cpf = Console.ReadLine();
            Console.Write("Digite o titular: ");
            string titular = Console.ReadLine();
            Console.Write("Insira a senha: ");
            string senha = Console.ReadLine();

            Usuario novoUsuario = new Usuario(cpf, titular, senha);
            novoUsuario.GerarNumeroDaConta(usuarios);
 
            SerializarJson(usuarios, fileName);
            Console.WriteLine("<------------------------------------------->");
            Console.WriteLine($"Usuario {novoUsuario.Titular} criado com sucesso!");
            Console.WriteLine("<------------------------------------------->");


        }
        static void MostrarTodosUsuarios(List<Usuario> usuarios)
        {
            Console.Clear();
            Console.WriteLine("<------------------------------------------------------------------------------->");
            usuarios.ForEach(usuario => Console.WriteLine("| Titular: {0,-20}| CPF: {1,-5}| Saldo: R${2,-10}| Numero da conta: {3,-5}|", usuario.Titular, usuario.Cpf, usuario.Saldo, usuario.NumeroDaConta));
            Console.WriteLine("<------------------------------------------------------------------------------->");
        }
        static void DeletarUsuario(List<Usuario> usuarios, string fileName)
        {
            Console.WriteLine("Digite o CPF");
            string cpfInput = Console.ReadLine();


            if (usuarios.Exists(usuario => usuario.Cpf == cpfInput))
            {
                int userIndex = usuarios.FindIndex(usuario => usuario.Cpf == cpfInput);
                Console.WriteLine("<------------------------------------------->");
                Console.WriteLine($"Usuario {usuarios[userIndex].Titular} deletado com sucesso");
                Console.WriteLine("<------------------------------------------->");
                usuarios.RemoveAt(userIndex);

                SerializarJson(usuarios, fileName);

            }
            else
            {
                Console.WriteLine("Usuario inexistente");
            }


        }
        static void MostrarUsuario(List<Usuario> usuarios)
        {
            Console.WriteLine("Digite o CPF");
            string cpfInput = Console.ReadLine();
            if (usuarios.Exists(usuario => usuario.Cpf == cpfInput))
            {
                int userIndex = usuarios.FindIndex(usuario => usuario.Cpf == cpfInput);

                Console.WriteLine($"Cpf: {usuarios[userIndex].Cpf} | Titular: {usuarios[userIndex].Titular}");
     
            }
            else
            {
                Console.WriteLine("Usuario inexistente");
            }



        }
        static void MostrarTotalBancario(List<Usuario> usuarios)
        {
            double soma = 0;
            usuarios.ForEach(usuario => soma += usuario.Saldo);
            Console.WriteLine($"Saldo total = R${soma:F2}");

        }
        static void Depositar(List<Usuario> usuarios,int userIndex,string fileName)
        {
            Console.WriteLine("Digite o valor que deseja depositar");
            double deposito = double.Parse(Console.ReadLine());
            if (deposito > 0)
            {
                usuarios[userIndex].Saldo += deposito;

                SerializarJson(usuarios, fileName);
                Console.WriteLine("<------------------------------------------->");
                Console.WriteLine($"Deposito no valor de R${deposito:F2} realidado com sucesso! ");
                Console.WriteLine($"Seu saldo atual é de R${usuarios[userIndex].Saldo:F2}");
                Console.WriteLine("<------------------------------------------->");
            }
            else
            {
                Console.WriteLine("Valor invalido");
            }
        }
        static void Sacar(List<Usuario> usuarios, int userIndex, string fileName)
        {
            Console.WriteLine("Digite o valor que deseja sacar");
            double saque = double.Parse(Console.ReadLine());
            if (usuarios[userIndex].Saldo >= saque)
            {
                usuarios[userIndex].Saldo -= saque;
                SerializarJson(usuarios, fileName);
                Console.WriteLine($"Saque no valor de R${saque:F2} realidado com sucesso! ");
                Console.WriteLine($"Seu saldo atual é de R${usuarios[userIndex].Saldo:F2}");
            }
            else
            {
                Console.WriteLine("saldo insuficiente");
            }
        }
        static void Transferir(List<Usuario> usuarios, int userIndex,string fileName)
        {
            Console.Write("Digite o numero da conta para quem deseja transferir: ");
            int numeroDaContaDestinatario = int.Parse(Console.ReadLine());
            ;
            if (usuarios.Exists(usuario => usuario.NumeroDaConta == numeroDaContaDestinatario))
            {

                Console.WriteLine("Digite o valor que deseja tranferir");
                double valor = double.Parse(Console.ReadLine());

                if (usuarios[userIndex].Saldo >= valor)
                {
                    int destinatarioIndex = usuarios.FindIndex(usuario=>usuario.NumeroDaConta==numeroDaContaDestinatario);
                    usuarios[userIndex].Saldo -= valor;
                    usuarios[destinatarioIndex].Saldo += valor;
                    SerializarJson(usuarios, fileName);
                    Console.WriteLine($"Tranferencia no valor de R${valor:F2} para {usuarios[destinatarioIndex].Titular} realizada com sucesso");
                }
                else
                {
                    Console.WriteLine("saldo insuficiente");
                }


            }
            else
            {
                Console.WriteLine("Conta inexistente");

            }

        }
        static void ManipularConta(List<Usuario> usuarios,string fileName)
        {
            Console.WriteLine("Digite o CPF");
            string cpfInput = Console.ReadLine();

            if (usuarios.Exists(usuario=>usuario.Cpf==cpfInput))
            {
                int userIndex = usuarios.FindIndex(usuario => usuario.Cpf == cpfInput);
                Console.WriteLine("Digite a senha");

                if (Console.ReadLine() == usuarios[userIndex].Senha)
                {
                    Console.Clear();
                    Console.WriteLine($"Seja bem vindo {usuarios[userIndex].Titular}, qual operação voce deseja realizar?  ");
                    int subOption;
                    do
                    {
                        MostrarSubMenu();
                        subOption = int.Parse(Console.ReadLine());

                        switch (subOption)
                        {
                            case 1:
                                Depositar(usuarios,userIndex,fileName);
                                break;
                            case 2:
                                Sacar(usuarios, userIndex, fileName);
                                break;
                            case 3:
                                Transferir(usuarios, userIndex,fileName);
                                break;
                            case 4:
                                Console.WriteLine("Saindo da conta...");
                                break;
                            default:
                                Console.WriteLine("Opção invalida, por favor digite opçoes entre 1, 2, 3 ou 4");
                                break;
                        }
                    } while (subOption != 4);

                }
                else
                {
                    Console.WriteLine("Senha invalida");
                }

            }
            else
            {
                Console.WriteLine("CPF não encontrado");
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
                        RegistrarNovoUsuario(usuarios, fileName);
                        break;
                    case 2:
                        DeletarUsuario(usuarios, fileName);
                        break;
                    case 3:
                        MostrarTodosUsuarios(usuarios);
                        break;
                    case 4:
                         MostrarUsuario(usuarios);
                        break;
                    case 5:
                        MostrarTotalBancario(usuarios);
                        break;
                    case 6:
                        ManipularConta(usuarios,fileName);
                        break;
                    default:
                        Console.WriteLine("Opção invalida, por vafor digite opçoes entre 0, 1, 2, 3, 4, 5 ou 6");
                        break;
                }

            } while (option != 0);



        }

    }
}