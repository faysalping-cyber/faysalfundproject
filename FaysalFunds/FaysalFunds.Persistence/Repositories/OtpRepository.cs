using FaysalFunds.Common;
using FaysalFunds.Common.Enums;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace FaysalFunds.Persistence.Repositories
{
    public class OtpRepository : BaseRepository<Otp>, IOtpRepository
    {
        private readonly Settings _settings;
        private readonly DbSet<Otp> _otpSet;
        private readonly TimeSpan _otpValidity;
        public OtpRepository(ApplicationDbContext dbContext, IOptions<Settings> settings) : base(dbContext)
        {
            _otpSet = dbContext.Set<Otp>();
            _settings = settings.Value;
            _otpValidity = TimeSpan.FromMinutes(_settings.OtpExpirationSeconds);
        }

        public async Task<OtpEnums> ValidateOtpAsync(long userId, string emailOtp, string phoneOtp)
        {
            var otpEntry = await _otpSet.FirstOrDefaultAsync(x =>
                x.USER_ID == userId &&
                x.EMAIL_OTP == emailOtp &&
                x.MOBILE_OTP == phoneOtp);

            if (otpEntry == null)
                //throw new ApiException(ApiErrorCodes.BadRequest, "Invalid OTP.");
                return OtpEnums.IsInValid;
            if (otpEntry.CREATED_AT.Add(_otpValidity) < DateTime.UtcNow)
                //throw new ApiException(ApiErrorCodes.BadRequest, "OTP has expired.");
                return OtpEnums.IsExpired;
            if (otpEntry.IS_EMAIL_VERIFIED == 1 && otpEntry.IS_MOBILE_VERIFIED == 1)
                //throw new ApiException(ApiErrorCodes.Forbidden, "OTP already verified.");
                return OtpEnums.IsVarified;
            otpEntry.IS_EMAIL_VERIFIED = 1;
            otpEntry.IS_MOBILE_VERIFIED = 1;
            await SaveChangesAsync();

            return OtpEnums.IsValid;
        }



        public async Task<int> SaveOtpsInDb(string email, string emailOtp, string mobile, string mobileOtp, byte isWhatsapp, long userId, string otpToken)
        {
            var expiry = DateTime.UtcNow.Add(_otpValidity);
            var OtpRecord = new Otp
            {
                USER_ID = userId,
                EMAIL_OTP = emailOtp,
                MOBILE_OTP = mobileOtp,
                CREATED_AT = DateTime.UtcNow,
                IS_WHATSAPP = isWhatsapp,
                OTP_TOKEN = otpToken
            };

            await AddAsync(OtpRecord);

            var result = await SaveChangesAsync();
            return result;
        }
    }
}
