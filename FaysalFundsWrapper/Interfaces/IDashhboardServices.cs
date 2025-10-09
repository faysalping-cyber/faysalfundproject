using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Models.AccountOpening;
using FaysalFundsWrapper.Models.Dashboard;

namespace FaysalFundsWrapper.Interfaces
{
    public interface IDashhboardServices
    {
        //Task<string> AddQuickAccessMenu(QuickAccesModel menu);
        // Task<List<QuickAccesModel>> GetAllQuickAccessMenu();
        // Task<UserQuickAccessResponseDto> GetUserQuickAccessMenus(long userId);
        // Task<bool> AddUserQuickAccess(UserQuickAccessDto request);
        // Task<bool> RemoveUserQuickAccess(UserQuickAccessDto request);

        Task<ApiResponseNoData> AddQuickAccessMenu(QuickAccesModel menu);

        Task<ApiResponseWithData<List<QuickAccesModel>>> GetAllQuickAccessMenu();


        Task<ApiResponseWithData<UserQuickAccessResponseDto>> GetUserQuickAccessMenus(long userId);

        Task<ApiResponseNoData> AddUserQuickAccess(UserQuickAccessDto request);

        Task<ApiResponseNoData> RemoveUserQuickAccess(UserQuickAccessDto request);
        Task<ApiResponseWithData<CheckBalance>> CheckBalance(CheckBalanceRequestModel request);
        Task<ApiResponseWithData<CheckProfitResponseModel>> CheckProfit(CheckProfitRequestModel request);
        Task<ApiResponseWithData<AvailableCustomerFolioResponseModel>> GetCustomerAvailableFolio(CustomerAvailableFolioRequest request);

        //Dashboard
        Task<ApiResponseWithData<InvestmentFundsDTO>> GetAllFunds(OnBoardingRequestModel request);
        Task<ApiResponseWithData<List<KpSlabDTO>>> GetAllKuickPayCharges(OnBoardingRequestModel request);
        Task<ApiResponseWithData<List<TransactionTypesGroupDTO>>> GetAllTransactionTypes(OnBoardingRequestModel request);

        //Task<ApiResponseWithData<List<TransactionFeaturesGroupedDTO>>> GetAllTransactionFeature(OnBoardingRequestModel request);
        Task<ApiResponseWithData<TransactionFeaturesGroupedDTO>> GetAllTransactionFeature(OnBoardingRequestModel request);

        Task<ApiResponseWithData<Dictionary<string, List<InvestmentInstructionsDTO>>>> GetInvestmentInstructions(OnBoardingRequestModel request);
        //Task<ApiResponseWithData<RaastAllowedOrNot>> CheckRaastTransactionAllowed(OnBoardingRequestModel request);
        Task<ApiResponseWithData<RaastAllowedOrNot>> CheckRaastTransactionAllowed(OnBoardingRequestModel request);

        Task<ApiResponseWithData<ViewTransaction>> GetViewTransactionLimit(OnBoardingRequestModel request);

        Task<ApiResponseWithData<TransactionFeaturesDTO>> GetTransactionFeatureById(TransactionID request);
        Task<ApiResponseWithData<FeaturePermissionResponse>> GetFeaturePermissions(FeaturePermissionRequestDTO request);
        Task<ApiResponseWithData<CalculateKuickPayDTO>> CalculateKuickPay(CalculateKuickPayLoad payload);
        Task<ApiResponseWithData<KuickPayFinalResponse>> GenerateKuickPayID(GenerateKuickPayIdDTO payload);
        Task<ApiResponseWithData<KuickPayReceiptDetailsDTO>> SaveKuickpayReceiptDetail(KuickPayReceiptPayload payload);
        Task<ApiResponseWithData<IBFTReceiptDetailDTO>> SaveIBFTReceiptDetail(IBFTReceiptPayload payload);

        //Task<ApiResponseWithData<TransactionReceiptPayload>> SaveTransactionReceiptDetail(TransactionReceiptPayload request);

    }
}
