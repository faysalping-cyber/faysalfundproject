using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Models.Raast;

namespace FaysalFundsWrapper.Services
{
    public class RaastService : IRaastService
    {
        private readonly string _controller = "Raast";
        private readonly IMainApiClient _mainApiClient;

        public RaastService(IMainApiClient mainApiClient)
        {
            _mainApiClient = mainApiClient;
        }
        public async Task<ApiResponseWithData<GenerateRaastIdResponseModel>> GenerateRaastId(GenerateRaastIdRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<GenerateRaastIdRequestModel, ApiResponseWithData<GenerateRaastIdResponseModel>>($"{_controller}/GenerateRaastId", request);
            return response;
        }
        public async Task<ApiResponseWithData<RaastIdsListResponseModel>> GetRaastIdsList(RaastIdsListRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<RaastIdsListRequestModel, ApiResponseWithData<List<RaastIdsModel>>>($"{_controller}/GetRaastIdsList", request);
            RaastIdsListResponseModel model = new()
            {
                IdsList = response.Data
            };
            return ApiResponseWithData<RaastIdsListResponseModel>.SuccessResponse(model);
        }
    }
}
