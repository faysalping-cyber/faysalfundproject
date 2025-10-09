using FaysalFunds.Common;
using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Domain.Interfaces;

namespace FaysalFunds.Application.Services
{
    public class JWTBlackListService
    {
        private readonly IJWTBlackListRepository _jWTBlackListRepository;

        public JWTBlackListService(IJWTBlackListRepository jWTBlackListRepository)
        {
            _jWTBlackListRepository = jWTBlackListRepository;
        }
        public async Task<ApiResponseWithData<bool>> IsTokenBlacklistedAsync(string token)
        {
            bool IsTokenBlacklisted = await _jWTBlackListRepository.IsTokenBlacklistedAsync(token);
            return ApiResponseWithData<bool>.SuccessResponse(IsTokenBlacklisted, "");
        }
        public async Task<ApiResponseNoData> BlacklistTokenAsync(string token, DateTime expirationDate)
        {
            var response = await _jWTBlackListRepository.BlackListToken(token, expirationDate);

            if (response < 1)
            {
                ApiResponseNoData.FailureResponse("Failed");
            }
            return ApiResponseNoData.SuccessResponse("Logout successful.");
        }
    }
}
