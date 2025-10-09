using FaysalFunds.Common.Enums;

namespace FaysalFunds.Common.ApiResponses
{
    public class ApiResponseNoData
    {
        public bool Status { get; set; }
        public ErrorIconEnums? ErrorIcon { get; set; }
        public string? ErrorHeading { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = "Successful";
        public object? Data { get; set; }

        public static ApiResponseNoData SuccessResponse(string message = "successful", string? errorHeading = null)
            => new ApiResponseNoData { Status = true, ErrorHeading = errorHeading, Message = message, Code = 200 };

        public static ApiResponseNoData FailureResponse(string message = "Failed", string errorHeading = "Error!", ErrorIconEnums? errorIcon = ErrorIconEnums.Generic)
            => new ApiResponseNoData { Status = false, Message = message, ErrorHeading = errorHeading, Code = ApiErrorCodes.Failed , ErrorIcon = errorIcon};
    }
}