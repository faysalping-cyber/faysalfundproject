using FaysalFunds.Common.Enums;

namespace FaysalFunds.Common.APIException
{
    public class ApiException : Exception
    {

        public bool Status { get; set; } = false;
        public ErrorIconEnums? ErrorIcon { get; set; }
        public string? ErrorHeading { get; set; } = "Error!";
        public int Code { get; set; } = ApiErrorCodes.Failed;
        public string Message { get; set; } = "Failed";
        public object? Data { get; set; }

        public ApiException(string errorMessage, string errorHeading = "Error!",ErrorIconEnums? errorIcon = ErrorIconEnums.Generic) : base(errorMessage) { Message = errorMessage; ErrorHeading = errorHeading; ErrorIcon = errorIcon; }
    }
}
