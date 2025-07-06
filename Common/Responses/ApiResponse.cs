namespace SchoolManagementSystem.Common.Responses;

public class ApiResponse<T>
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }

    public ApiResponse(bool succeeded, string message, T? data)
    {
        Succeeded = succeeded;
        Message = message;
        Data = data;
    }

    public static ApiResponse<T> Success(T data, string message = "Request successful.")
    {
        return new ApiResponse<T>(true, message, data);
    }

    public static ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T>(false, message, default);
    }
}