using FaysalFunds.Application.DTOs;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FaysalFunds.Application.Services
{
    public class RefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly int expiryDay = 7;
        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IAccountRepository accountRepository)
        {

            _refreshTokenRepository = refreshTokenRepository;
            _accountRepository = accountRepository;
        }
        public async Task<ApiResponseWithData<string>> SaveRefreshToken(string email)
        {
           var refreshToken = GenerateRefreshToken();
            var userId = await _accountRepository.GetUserIdByEmail(email);
            await RevokeUserTokensAsync(email);
            RefreshToken refresh = new ()
            {
                TOKEN = refreshToken,
                ACCOUNTID = userId,
                EXPIRYDATE = DateTime.UtcNow.AddDays(expiryDay),
                EMAIL = email
            };
            var response = await _refreshTokenRepository.SaveRefreshToken(refresh);
            if (response <= 0)
            {
                ApiResponseWithData<string>.FailureResponse("please contact admin");
            }
            return ApiResponseWithData<string>.SuccessResponse(refreshToken);
        }
        public async Task<ApiResponseNoData> RevokeUserTokensAsync(string email)
        {
           var userId= await _accountRepository.GetUserIdByEmail(email);
            await _refreshTokenRepository.RevokeUserTokensAsync(userId);
            return ApiResponseNoData.SuccessResponse();
        }
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        public async Task<ApiResponseWithData<RefreshTokenResponse>> GetByTokenAsync(string refreshToken)
        {
            var refreshTokenInDb = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (refreshTokenInDb == null)
            {
                ApiResponseWithData<RefreshTokenResponse>.FailureResponse("Refresh token does not exist.");
            }
            RefreshTokenResponse refreshTokenResponse = new()
            {
                Email = refreshTokenInDb.EMAIL,
                ExpiryDate =refreshTokenInDb.EXPIRYDATE,
                IsRevoked = refreshTokenInDb.ISREVOKED,
                Token = refreshTokenInDb.TOKEN
            };
            return ApiResponseWithData<RefreshTokenResponse>.SuccessResponse(refreshTokenResponse);
        }
    }
}