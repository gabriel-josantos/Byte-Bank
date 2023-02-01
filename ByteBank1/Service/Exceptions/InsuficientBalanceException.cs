using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Service.Exceptions
{
    public class InsuficientBalanceException:AccountException
    {
        public InsuficientBalanceException() : base("Saldo insuficiente")
        {

        }
    }
}
