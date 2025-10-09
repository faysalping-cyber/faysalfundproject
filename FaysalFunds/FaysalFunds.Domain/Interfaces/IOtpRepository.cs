using FaysalFunds.Common.Enums;
using FaysalFunds.Domain.Entities;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IOtpRepository
    {
        Task<int> SaveOtpsInDb(string email, string emailOtp, string mobile, string mobileOtp, byte isWhatsapp, long userId, string otpToken);
        Task<OtpEnums> ValidateOtpAsync(long userId, string emailOtp, string phoneOtp);
    }
}