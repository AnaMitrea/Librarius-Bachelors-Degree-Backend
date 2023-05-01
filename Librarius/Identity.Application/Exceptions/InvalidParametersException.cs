namespace Identity.Application.Exceptions;

public class InvalidParametersException: Exception
{
    public InvalidParametersException(): base("Invalid parameters.") { }

    public InvalidParametersException(string message) : base(message) { }

    public InvalidParametersException(string message, Exception innerException) : base(message, innerException) { }
}