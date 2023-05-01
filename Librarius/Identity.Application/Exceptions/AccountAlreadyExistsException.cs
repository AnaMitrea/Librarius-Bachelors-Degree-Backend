namespace Identity.Application.Exceptions;

public class AccountAlreadyExistsException : Exception
{
    public AccountAlreadyExistsException(): base("Account already exists.") { }

    public AccountAlreadyExistsException(string message) : base(message) { }

    public AccountAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
}