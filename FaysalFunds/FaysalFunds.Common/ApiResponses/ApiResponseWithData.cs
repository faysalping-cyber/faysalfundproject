using FaysalFunds.Common.Enums;

namespace FaysalFunds.Common
{
    public class ApiResponseWithData<T>
    {
        public bool Status { get; set; }
        public ErrorIconEnums? ErrorIcon { get; set; }
        public string? ErrorHeading { get; set; } 
        public int Code { get; set; }
        public string Message { get; set; } = "Successful";
        public T? Data { get; set; }


        public static ApiResponseWithData<T> SuccessResponse(T data, string message = "Successful")
            => new ApiResponseWithData<T> { Status = true, Data = data, Message = message, Code = 200 };

        public static ApiResponseWithData<T> FailureResponse(string message = "Failed", string? errorHeading = "Error!", ErrorIconEnums? errorIcon = ErrorIconEnums.Generic)
            => new ApiResponseWithData<T> { Status = false, ErrorHeading = errorHeading, Message = message, Data = default, Code = ApiErrorCodes.Failed, ErrorIcon=errorIcon };

    }
}
