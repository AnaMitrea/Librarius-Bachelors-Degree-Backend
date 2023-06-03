namespace Trophy.API.Models;

public class ApiValidationError
{
    public ApiValidationError(string? field, string message)
    {
        Field = field != string.Empty ? field : null;
        Message = message;
    }

    public string? Field { get; }

    public string Message { get; }
}