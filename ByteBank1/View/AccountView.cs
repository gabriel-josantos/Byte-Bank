using ByteBank.Model.DTO;
using ByteBank.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteBank.Service;

namespace ByteBank.View
{
    public class AccountView
    {
        public static LoginFormDto MenuLogin()
        {

            Console.Clear();
            Console.WriteLine("Digite seu cpf");
            string cpfInput = Console.ReadLine(); ;

            Console.WriteLine("Digite sua senha");
            string passInput = Console.ReadLine();

            return new LoginFormDto
            {
                Cpf = cpfInput,
                Password = passInput,
                Moment= DateTime.Now,

            };


        }
    }
}
