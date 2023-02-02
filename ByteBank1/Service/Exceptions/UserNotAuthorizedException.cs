using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Service.Exceptions
{
    public class UserNotAuthorizedException : AccountException
    {
       public UserNotAuthorizedException(string msg) : base(msg) { }
    }
}
