using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Model.DTO
{
    public class LoginFormDto
    {
        public string Cpf { get; set; }

        public string Password { get; set; }    

        public DateTime Moment { get; set; }
    }
}
