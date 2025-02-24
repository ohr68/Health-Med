namespace HealthMed.WebApi.Common;

public class ApiResponseWithData<T> : ApiResponse
{
    public T? Data { get; set; }

    public ApiResponseWithData()
    {
        
    }

    public ApiResponseWithData(T? data, bool success, string message)
    {
        Data = data;
        Success = success;
        Message = message;
    }
}