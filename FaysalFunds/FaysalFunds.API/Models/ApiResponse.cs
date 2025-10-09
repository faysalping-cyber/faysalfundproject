using FaysalFunds.Common;

namespace FaysalFunds.API.Models
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = "Successful";
        public object? Data { get; set; }


        public static ApiResponse<T> SuccessResponse(T data, string message = "")
            => new ApiResponse<T> { Status = true, Data = data, Message = message, Code = ApiErrorCodes.Failed };

        public static ApiResponse<T> FailureResponse(string message)
            => new ApiResponse<T> { Status = false, Message = message, Data = null, Code = 200 };

    }
}
