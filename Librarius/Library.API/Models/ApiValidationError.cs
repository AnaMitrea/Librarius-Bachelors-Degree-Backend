namespace Library.API.Models;

public class ApiValidationError
{
    public ApiValidationError(string? field, string message)
    {
        this.Field = field != string.Empty ? field : null;
        this.Message = message;
    }

    public string? Field { get; }

    public string Message { get; }
}