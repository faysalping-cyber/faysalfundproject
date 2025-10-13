using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Models.Raast;

namespace FaysalFundsWrapper.Interfaces
{
    public interface IRaastService
    {
        Task<ApiResponseWithData<GenerateRaastIdResponseModel>> GenerateRaastId(GenerateRaastIdRequestModel request);
        Task<ApiResponseWithData<RaastIdsListResponseModel>> GetRaastIdsList(RaastIdsListRequestModel request);
    }
}
