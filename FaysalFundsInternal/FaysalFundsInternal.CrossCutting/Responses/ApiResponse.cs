using FaysalFundsInternal.CrossCutting.Constants;

namespace FaysalFundsInternal.CrossCutting.Responses
{
    public class ApiResponse<T> : IApiStructure
    {
        public bool Status { get; set; }
        public string Message { get; set; } = "Successful";
        public int Code { get; set; }
        public T? Data { get; set; }

        public object? GetData() => Data;

        public static ApiResponse<T> Success(T? data = default, string message = "Successful") =>
            new()
            {
                Status = true,
                Data = data,
                Message = message,
                Code = 200
            };

        public static ApiResponse<T> Failure(string message = "Failed") =>
            new()
            {
                Status = false,
                Message = message,
                Data = default,
                Code = ApiErrorCodes.Failed
            };
    }
}
