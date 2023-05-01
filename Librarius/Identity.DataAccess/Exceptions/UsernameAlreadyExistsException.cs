namespace Identity.DataAccess.Exceptions;

public class UsernameAlreadyExistsException: Exception
{
    public UsernameAlreadyExistsException(): base("Username already exists.") { }

    public UsernameAlreadyExistsException(string message) : base(message) { }

    public UsernameAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
}