using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Entities
{
    public class Usuario
    {

        public Usuario(string cpf, string titular, string senha)
        {
            Cpf = cpf;
            Titular = titular;
            Senha = senha;

        }
        public string Cpf { get; private set; }
        public string Titular { get; set; }
        public string Senha { get; set; }
        public int NumeroDaConta { get; set; }
        public double Saldo { get; set; }

        /*Não gerei o numero da conta dentro da construtora pq preciso checar se
        algum usuario ja possui o numero da conta a ser gerado, e caso sim tentar novamente*/
        public void GerarNumeroDaConta(List<Usuario> usuarios)
        {
            Random rnd = new Random();
            int numeroGerado = rnd.Next(100, 1000);

            while (usuarios.Exists(usuario => usuario.NumeroDaConta == numeroGerado))
            {
                numeroGerado = rnd.Next(100, 1000);
            }

            NumeroDaConta = numeroGerado;

        }

        //fazer validação com property e quando não, usar auto property

    }
}
