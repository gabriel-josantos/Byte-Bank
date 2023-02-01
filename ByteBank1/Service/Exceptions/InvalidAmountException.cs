using System.Runtime.Serialization;

namespace ByteBank.Service.Exceptions;

public class InvalidAmountException : Exception
{
    public InvalidAmountException(string msg) : base(msg) { }

}
