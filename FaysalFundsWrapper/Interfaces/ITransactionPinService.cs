using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Models.AccountOpening;

namespace FaysalFundsWrapper.Interfaces
{
    public interface ITransactionPinService
    {
        Task<ApiResponseNoData> ChangeTransactionPin(ChangeTPinRequest request);
        Task<ApiResponseNoData> ForgotTransactionPin(ForgotTPinRequest request);
        Task<ApiResponseNoData> GenerateTransactionPin(GenerateTPinRequest request);
        Task<ApiResponseNoData> VerifyTransactionPin(VerifyTpinRequest request);
        Task<ApiResponseNoData> VerifyUserBeforeForgotTpin(VerifyUserBeforeForgotTpin request);
    }
}
