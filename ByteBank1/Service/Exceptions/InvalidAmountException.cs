
namespace ByteBank.Service.Exceptions;

public class InvalidAmountException : AccountException
{
    public InvalidAmountException(string msg) : base(msg) { }

}
