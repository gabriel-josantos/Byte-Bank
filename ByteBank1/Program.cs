using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ByteBank1
{
    public class Program
    {

        static void ShowMenu()
        {
            Console.WriteLine("1 - Inserir novo usuário");
            Console.WriteLine("2 - Deletar um usuário");
            Console.WriteLine("3 - Listar todas as contas registradas");
            Console.WriteLine("4 - Detalhes de um usuário");
            Console.WriteLine("5 - Total armazenado no banco");
            Console.WriteLine("6 - Manipular a conta");
            Console.WriteLine("0 - Para sair do programa");
            Console.Write("Digite a opção desejada: ");
        }

        static void ShowSubMenu()
        {
            Console.WriteLine("1 - Depositar");
            Console.WriteLine("2 - Sacar");
            Console.WriteLine("3 - Trasferir");
            Console.WriteLine("4 - Sair");
        }

        static void RegistrarNovoUsuario(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos)
        {
            Console.WriteLine("Digite o cpf: ");
            cpfs.Add(Console.ReadLine());
            Console.WriteLine("Digite o titular: ");
            titulares.Add(Console.ReadLine());
            Console.WriteLine("Insira a senha: ");
            senhas.Add(Console.ReadLine());
            saldos.Add(0);


        }

        static void ListarTodasAsContas(List<string> cpfs, List<string> titulares, List<double> saldos)
        {
            for (int i = 0; i < cpfs.Count; i++)
            {
                Console.WriteLine($"CPF = {cpfs[i]}  | Titular = {titulares[i]} | Saldo = R${saldos[i]:F2}");
            }
        }

        static void DeletarUsuario(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos)
        {
            Console.WriteLine("Digite o CPF");
            string cpf = Console.ReadLine();
            if (cpfs.Contains(cpf))
            {
                int index = cpfs.IndexOf(cpf);
                string usuario = titulares[index];


                cpfs.RemoveAt(index);
                titulares.RemoveAt(index);
                saldos.RemoveAt(index);
                senhas.RemoveAt(index);

                Console.WriteLine($"Usuario {usuario} deletado com sucesso");


            }
            else
            {
                Console.WriteLine("Usuario inexistente");
            }


        }

        static void MostrarUsuario(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos)
        {
            Console.WriteLine("Digite o CPF");
            string cpf = Console.ReadLine();
            if (cpfs.Contains(cpf))
            {
                int index = cpfs.IndexOf(cpf);
                Console.WriteLine($"CPF = {cpfs[index]}  | Titular = {titulares[index]} | Saldo = R${saldos[index]:F2}  | Senha = {senhas[index]:F2}");
            }
            else
            {
                Console.WriteLine("Usuario inexistente");
            }


        }

        static void MostrarTotalBancario(List<double> saldos)
        {
            double soma = 0;
            foreach (double saldo in saldos)
            {
                soma += saldo;
            }
            Console.WriteLine($"Saldo total = R${soma:F2}");

        }

        static void Depositar(List<double> saldos, int userIndex)
        {
            Console.WriteLine("Digite o valor que deseja depositar");
            double deposito = double.Parse(Console.ReadLine());
            if (deposito > 0)
            {
                saldos[userIndex] += deposito;
                Console.WriteLine($"Deposito no valor de R${deposito:F2} realidado com sucesso! ");
                Console.WriteLine($"Seu saldo atual é de R${saldos[userIndex]:F2}");
            }
            else
            {
                Console.WriteLine("Valor invalido");
            }
        }
        static void Sacar(List<double> saldos, int userIndex)
        {
            Console.WriteLine("Digite o valor que deseja sacar");
            double saque = double.Parse(Console.ReadLine());
            if (saldos[userIndex] >= saque)
            {
                saldos[userIndex] -= saque;
                Console.WriteLine($"Saque no valor de R${saque:F2} realidado com sucesso! ");
                Console.WriteLine($"Seu saldo atual é de R${saldos[userIndex]:F2}");
            }
            else
            {
                Console.WriteLine("saldo insuficiente");
            }
        }
        static void Transferir(List<double> saldos, List<string> titulares, int userIndex)
        {
            Console.WriteLine("Digite o titular para quem deseja tranferir");
            string destinatario = Console.ReadLine();

            if (titulares.Contains(destinatario))
            {

                Console.WriteLine("Digite o valor que deseja tranferir");
                double valor = double.Parse(Console.ReadLine());

                if (saldos[userIndex] >= valor)
                {
                    int destinatarioIndex = titulares.IndexOf(destinatario);
                    saldos[userIndex] -= valor;
                    saldos[destinatarioIndex] += valor;
                    Console.WriteLine($"Tranferencia no valor de R${valor:F2} para {titulares[destinatarioIndex]} realizada com sucesso");
                }
                else
                {
                    Console.WriteLine("saldo insuficiente");
                }


            }
            else
            {
                Console.WriteLine("Titular inexistente");

            }

        }


        static void ManipularConta(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos)
        {
            Console.WriteLine("Digite o CPF");
            string cpf = Console.ReadLine();

            if (cpfs.Contains(cpf))
            {
                int userIndex = cpfs.IndexOf(cpf);
                Console.WriteLine("Digite a senha");

                if (Console.ReadLine() == senhas[userIndex])
                {
                    Console.WriteLine($"Seja bem vindo {titulares[userIndex]}, qual operação voce deseja realizar?  ");
                    int subOption;
                    do
                    {
                        ShowSubMenu();
                        subOption = int.Parse(Console.ReadLine());

                        switch (subOption)
                        {
                            case 1:
                                Depositar(saldos, userIndex);
                                break;
                            case 2:
                                Sacar(saldos, userIndex);
                                break;
                            case 3:
                                Transferir(saldos, titulares, userIndex);
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

            Console.WriteLine("Antes de começar a usar, vamos configurar alguns valores: ");

            Console.Write("Digite a quantidade de Usuários: ");
            int quantidadeDeUsuarios = int.Parse(Console.ReadLine());

            List<string> cpfs = new List<string>();
            List<string> titulares = new List<string>();
            List<string> senhas = new List<string>();
            List<double> saldos = new List<double>();

            int option;

            do
            {
                ShowMenu();
                option = int.Parse(Console.ReadLine());

                Console.WriteLine("-----------------");

                switch (option)
                {
                    case 0:
                        Console.WriteLine("Estou encerrando o programa...");
                        break;
                    case 1:
                        RegistrarNovoUsuario(cpfs, titulares, senhas, saldos);
                        break;
                    case 2:
                        DeletarUsuario(cpfs, titulares, senhas, saldos);
                        break;
                    case 3:
                        ListarTodasAsContas(cpfs, titulares, saldos);
                        break;
                    case 4:
                        MostrarUsuario(cpfs, titulares, senhas, saldos);
                        break;
                    case 5:
                        MostrarTotalBancario(saldos);
                        break;
                    case 6:
                        ManipularConta(cpfs, titulares, senhas, saldos);
                        break;
                    default:
                        Console.WriteLine("Opção invalida, por vafor digite opçoes entre 0, 1, 2, 3, 4, 5 ou 6");
                        break;
                }

                Console.WriteLine("-----------------");

            } while (option != 0);



        }

    }
}