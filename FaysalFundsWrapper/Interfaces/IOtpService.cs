using FaysalFundsWrapper.Models;

namespace FaysalFundsWrapper.Interfaces
{
    public interface IOtpService
    {
        Task<ApiResponseNoData> GenerateOTP(GenerateOTPModel request);
        Task<ApiResponseNoData> ValidateOTP(ValidateOTPModel request);

    }
}
