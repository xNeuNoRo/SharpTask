namespace SharpTask.Domain.Common;

public class ApiError
{
    public string Code { get; init; }
    public string Message { get; init; }

    public ApiError(string code, string message)
    {
        Code = code;
        Message = message;
    }
}
