using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;


namespace FaysalFundsWrapper.Services
{
    public class TransactionPinService : ITransactionPinService
    {
        private readonly TokenProviderService _tokenProviderService;

        private readonly string _controller = "AccountOpening";
        private readonly IMainApiClient _mainApiClient;

        public TransactionPinService(IMainApiClient mainApiClient, TokenProviderService tokenProviderService)
        {
            _mainApiClient = mainApiClient;
            _tokenProviderService = tokenProviderService;

        }
        public async Task<ApiResponseNoData> GenerateTransactionPin(GenerateTPinRequest request)
        {
            var response = await _mainApiClient.PostAsync<GenerateTPinRequest, ApiResponseNoData> ($"{_controller}/GenerateTPin", request);

            return response;
        }
        public async Task<ApiResponseNoData> VerifyTransactionPin(VerifyTpinRequest request)
        {
            var response = await _mainApiClient.PostAsync<VerifyTpinRequest,ApiResponseNoData>($"{_controller}/VerifyTPin", request);
            return response;
        }
        public async Task<ApiResponseNoData> ChangeTransactionPin(ChangeTPinRequest request)
        {
            var response = await _mainApiClient.PostAsync<ChangeTPinRequest, ApiResponseNoData>($"{_controller}/ChangeTPin", request);
            return response;
        }
        public async Task<ApiResponseNoData> ForgotTransactionPin(ForgotTPinRequest request)
        {
            var response = await _mainApiClient.PostAsync<ForgotTPinRequest, ApiResponseNoData>($"{_controller}/ForgotTPin", request);
            return response;
        }

        public async Task<ApiResponseNoData> VerifyUserBeforeForgotTpin(VerifyUserBeforeForgotTpin request)
        {
            var response = await _mainApiClient.PostAsync<VerifyUserBeforeForgotTpin, ApiResponseNoData>($"{_controller}/VerifyUserBeforeTpin", request);
            return response;
        }
    }
}
