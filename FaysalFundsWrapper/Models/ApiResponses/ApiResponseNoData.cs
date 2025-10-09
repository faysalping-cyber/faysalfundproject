using FaysalFundsWrapper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFundsWrapper.Models
{
    public class ApiResponseNoData
    {
        public bool Status { get; set; }
        public string? ErrorHeading { get; set; } = "Error!";
        public ErrorIconEnums? ErrorIcon { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = "Successful";
        public object? Data { get; set; }

        public static ApiResponseNoData SuccessResponse(string message = "successful",string? errorHeading = null)
            => new ApiResponseNoData { Status = true, ErrorHeading = errorHeading, Message = message, Code = 200 };

        public static ApiResponseNoData FailureResponse(string message="Failed", string? errorHeading = "Error!",ErrorIconEnums? errorIconEnums = ErrorIconEnums.Generic)
            => new ApiResponseNoData { Status = false, ErrorHeading = errorHeading, Message = message, Code = ApiErrorCodes.Failed,ErrorIcon=errorIconEnums};
    }
}
