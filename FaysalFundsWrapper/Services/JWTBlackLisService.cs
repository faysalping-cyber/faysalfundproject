using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using System.Net.Http;

namespace FaysalFundsWrapper.Services
{
    public class JWTBlackLisService: IJWTBlackLisService
    {
        private readonly string _controller = "JWT";
        private readonly IMainApiClient _mainApiClient;
        public JWTBlackLisService(IMainApiClient mainApiClient)
        {
            _mainApiClient = mainApiClient;
        }
        public async Task<string> BlacklistTokenAsync(BlackListTokenRequestModel request)
        {
            var response = await _mainApiClient.PostAsync($"{_controller}/BlacklistTokenAsync", request);
            return response;
        }
        public async Task<string> IsTokenBlacklistedAsync(string request)
        {
            CheckIsTokenBlackListedModel obj = new CheckIsTokenBlackListedModel()
            {
                Token = request
            };
            var response = await _mainApiClient.PostAsync($"{_controller}/IsTokenBlacklistedAsync", obj);
            return response;
        }
    }
}