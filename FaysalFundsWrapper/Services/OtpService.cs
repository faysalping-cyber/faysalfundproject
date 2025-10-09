using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;

namespace FaysalFundsWrapper.Services
{
    public class OtpService : IOtpService
    {
        private readonly string _controller = "otp";
        private readonly IMainApiClient _mainApiClient;
        private readonly TokenProviderService _tokenProviderService;
        public OtpService(IMainApiClient mainApiClient, TokenProviderService tokenProviderService)
        {
            _mainApiClient = mainApiClient;
            _tokenProviderService = tokenProviderService;
        }

        public async Task<ApiResponseNoData> GenerateOTP(GenerateOTPModel request)
        {
            var otpToken = _tokenProviderService.CreateOtpToken(request.Email);
            request.OtpToken =otpToken;
            var response = await _mainApiClient.PostAsync<GenerateOTPModel,ApiResponseNoData>($"{_controller}/GenerateOtp", request);
            return response;
        }

        public async Task<ApiResponseNoData> ValidateOTP(ValidateOTPModel request)
        {
            var email =_tokenProviderService.ValidateOtpToken(request.OtpToken);
            request.Email = email;
            var response = await _mainApiClient.PostAsync<ValidateOTPModel, ApiResponseNoData>($"{_controller}/ValidateOTP", request);
            return response;
        }
    }
}
