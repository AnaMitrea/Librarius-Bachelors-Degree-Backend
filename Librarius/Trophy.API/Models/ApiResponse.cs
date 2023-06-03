namespace Trophy.API.Models;


public class ApiResponse<T>
{
    private ApiResponse(bool succeeded, T result, IEnumerable<ApiValidationError> errors)
    {
        Succeeded = succeeded;
        Result = result;
        Errors = errors;
    }

    public bool Succeeded { get; }

    public T Result { get; }

    public IEnumerable<ApiValidationError> Errors { get; }

    public static ApiResponse<T> Success(T result)
    {
        return new ApiResponse<T>(true, result, new List<ApiValidationError>());
    }

    public static ApiResponse<T> Fail(IEnumerable<ApiValidationError> errors)
    {
        return new ApiResponse<T>(false, default, errors);
    }
}