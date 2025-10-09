using FaysalFundsWrapper.Models;

namespace FaysalFundsWrapper.Interfaces
{
    public interface IJWTBlackLisService
    {
        Task<string> BlacklistTokenAsync(BlackListTokenRequestModel request);
        Task<string> IsTokenBlacklistedAsync(string request);
    }
}
