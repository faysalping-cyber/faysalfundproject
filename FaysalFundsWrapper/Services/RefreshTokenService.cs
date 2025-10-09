using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using System.Net.Http;
using System.Security.Cryptography;

namespace FaysalFundsWrapper.Services
{
    public class RefreshTokenService
    {
        private readonly string _controller = "RefreshToken";
        private readonly IMainApiClient _mainApiClient;
        public RefreshTokenService(IMainApiClient mainApiClient)
        {
            _mainApiClient = mainApiClient;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        public async Task<string> SaveRefreshToken(RefreshTokenRequestModel request)
        {
            var response = await _mainApiClient.PostAsync($"{_controller}/SaveRefreshToken", request);
            return response;
        }
        public async Task<string> RevokeUserTokensAsync(RevokeTokenRequestModel request)
        {
            var response = await _mainApiClient.PostAsync($"{_controller}/RevokeUserTokensAsync", request);
            return response;
        }
        public async Task<string> GetByTokenAsync(TokenRequest request)
        {
            var response = await _mainApiClient.PostAsync($"{_controller}/refresh-token", request);
            return response;
        }
    }
}
