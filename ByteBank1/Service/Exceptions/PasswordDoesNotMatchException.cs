using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Service.Exceptions
{
    public class PasswordDoesNotMatchException:AccountException
    {
        public PasswordDoesNotMatchException() : base("A senha está incorreta") { }
        public PasswordDoesNotMatchException(string msg) : base(msg) { }
    }
}
