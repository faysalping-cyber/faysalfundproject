using FaysalFundsInternal.CrossCutting.Responses;
using FaysalFundsInternal.Infrastructure.DTOs.Raast;

namespace FaysalFundsInternal.Infrastructure.Raast
{
    public interface IRaastService
    {
        Task<ApiResponse<GenerateIbanResponse>> GenerateIban(GenerateIbanRequestModel request);
        Task<ApiResponse<List<IbanListModel>>> GetIbanList(ListIbanRequestModel request);
    }
}
