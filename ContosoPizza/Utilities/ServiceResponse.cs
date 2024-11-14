public class ServiceResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }

    // Статические методы для создания успешного и неудачного ответа
    public static ServiceResponse SuccessResponse(string message, object? data = null)
    {
        return new ServiceResponse
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ServiceResponse FailureResponse(string message)
    {
        return new ServiceResponse
        {
            Success = false,
            Message = message,
            Data = null
        };
    }
}
