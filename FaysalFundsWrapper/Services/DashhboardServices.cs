using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Models.AccountOpening;
using FaysalFundsWrapper.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
namespace FaysalFundsWrapper.Services
{
    public class DashhboardServices : IDashhboardServices
    {
        
        private readonly string _controller = "DashBoard";
        private readonly IMainApiClient _mainApiClient;
        private readonly TokenProviderService _tokenProviderService;

        public DashhboardServices(IMainApiClient mainApiClient, TokenProviderService tokenProviderService)
        {
            _mainApiClient = mainApiClient;
            _tokenProviderService = tokenProviderService;

        }
       
        public async Task<ApiResponseNoData> AddQuickAccessMenu(QuickAccesModel menu)
        {
            return await _mainApiClient.PostAsync<QuickAccesModel, ApiResponseNoData>(
                $"{_controller}/AddQuickAccessMenu", menu
            );
        }

        public async Task<ApiResponseWithData<List<QuickAccesModel>>> GetAllQuickAccessMenu()
        {
            return await _mainApiClient.GetAsync<ApiResponseWithData<List<QuickAccesModel>>>(
                $"{_controller}/GetActiveQuickAccessMenu"
            );
        }

        public async Task<ApiResponseWithData<UserQuickAccessResponseDto>> GetUserQuickAccessMenus(long userId)
        {
            var request = new OnBoardingRequestModel { UserId = userId };
            return await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<UserQuickAccessResponseDto>>(
                $"{_controller}/GetUserQuickAccessMenus", request
            );
        }

        public async Task<ApiResponseNoData> AddUserQuickAccess(UserQuickAccessDto request)
        {
            return await _mainApiClient.PostAsync<UserQuickAccessDto, ApiResponseNoData>(
                $"{_controller}/AddUserQuickAccess", request
            );
        }

        public async Task<ApiResponseNoData> RemoveUserQuickAccess(UserQuickAccessDto request)
        {
            return await _mainApiClient.PostAsync<UserQuickAccessDto, ApiResponseNoData>(
                $"{_controller}/RemoveUserQuickAccess", request
            );
        }

        public async Task<ApiResponseWithData<CheckBalance>> CheckBalance(CheckBalanceRequestModel request)
        {
            return await _mainApiClient.PostAsync<CheckBalanceRequestModel, ApiResponseWithData<CheckBalance>>(
                $"{_controller}/CheckBalance", request
            );
        }

        public async Task<ApiResponseWithData<CheckProfitResponseModel>> CheckProfit(CheckProfitRequestModel request)
        {
            return await _mainApiClient.PostAsync<CheckProfitRequestModel, ApiResponseWithData<CheckProfitResponseModel>>(
                $"{_controller}/CheckProfit", request
            );
        }

        public async Task<ApiResponseWithData<AvailableCustomerFolioResponseModel>> GetCustomerAvailableFolio(CustomerAvailableFolioRequest request)
        {
            return await _mainApiClient.PostAsync<CustomerAvailableFolioRequest, ApiResponseWithData<AvailableCustomerFolioResponseModel>>(
                $"{_controller}/GetCustomerAvailableFolio", request
            );
        }

        //DashBoard
        public async Task<ApiResponseWithData<InvestmentFundsDTO>> GetAllFunds(OnBoardingRequestModel request)
        {
            return await _mainApiClient.GetAsync<ApiResponseWithData<InvestmentFundsDTO>>(
                 $"{_controller}/GetInvestmentFunds"
             );
        }
        public async Task<ApiResponseWithData<List<KpSlabDTO>>> GetAllKuickPayCharges(OnBoardingRequestModel request)
        {
            return await _mainApiClient.GetAsync<ApiResponseWithData<List<KpSlabDTO>>>(
                 $"{_controller}/GetKuickPayCharges"
             );
        }
        public async Task<ApiResponseWithData<List<TransactionTypesGroupDTO>>> GetAllTransactionTypes(OnBoardingRequestModel request)
        {
            return await _mainApiClient.GetAsync<ApiResponseWithData<List<TransactionTypesGroupDTO>>>(
                 $"{_controller}/GetTransactionTypes"
             );
        }
        //public async Task<ApiResponseWithData<List<TransactionFeaturesGroupedDTO>>> GetAllTransaconFeature(OnBoardingRequestModel request)
        //{
        //    return await _mainApiClient.GetAsync<ApiResponseWithData<List<TransactionFeaturesGroupedDTO>>>(
        //         $"{_controller}/TransactionFeature"
        //     );
        //}
        public async Task<ApiResponseWithData<TransactionFeaturesGroupedDTO>> GetAllTransactionFeature(OnBoardingRequestModel request)
        {
            return await _mainApiClient.GetAsync<ApiResponseWithData<TransactionFeaturesGroupedDTO>>(
                $"{_controller}/TransactionFeature"
            );
        }

        public async Task<ApiResponseWithData<Dictionary<string, List<InvestmentInstructionsDTO>>>> GetInvestmentInstructions(OnBoardingRequestModel request)
        {
            return await _mainApiClient.GetAsync<ApiResponseWithData<Dictionary<string, List<InvestmentInstructionsDTO>>>>(
                 $"{_controller}/GetInvestmentInstructions"
             );
        }
       
        public async Task<ApiResponseWithData<RaastAllowedOrNot>> CheckRaastTransactionAllowed(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<RaastAllowedOrNot>>($"{_controller}/CheckRaastTransactionAllowed", request);
            return response;
        }

        public async Task<ApiResponseWithData<ViewTransaction>> GetViewTransactionLimit(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<ViewTransaction>>($"{_controller}/GetViewTransactionLimit", request);
            return response;
        }

        public async Task<ApiResponseWithData<TransactionFeaturesDTO>> GetTransactionFeatureById(TransactionID request)
        {
            return await _mainApiClient.PostAsync<TransactionID, ApiResponseWithData<TransactionFeaturesDTO>>(
                $"{_controller}/GetTransaconFeatureById", request
            );
        }
        public async Task<ApiResponseWithData<FeaturePermissionResponse>> GetFeaturePermissions(FeaturePermissionRequestDTO request)
        {
            return await _mainApiClient.PostAsync<FeaturePermissionRequestDTO, ApiResponseWithData<FeaturePermissionResponse>>(
                $"{_controller}/check-permissions", request
            );
        }

        public async Task<ApiResponseWithData<CalculateKuickPayDTO>> CalculateKuickPay(CalculateKuickPayLoad payload)
        {
            var response = await _mainApiClient.PostAsync<CalculateKuickPayLoad, ApiResponseWithData<CalculateKuickPayDTO>>(
                $"{_controller}/CalculateKuickPay", payload);
            return response;
        }
        public async Task<ApiResponseWithData<KuickPayFinalResponse>> GenerateKuickPayID(GenerateKuickPayIdDTO payload)
        {
            var response = await _mainApiClient.PostAsync<GenerateKuickPayIdDTO, ApiResponseWithData<KuickPayFinalResponse>>(
                $"{_controller}/GenerateKuickPayID", payload);
            return response;
        }
        public async Task<ApiResponseWithData<KuickPayReceiptDetailsDTO>> SaveKuickpayReceiptDetail(KuickPayReceiptPayload payload)
        {
            var response = await _mainApiClient.PostAsync<KuickPayReceiptPayload, ApiResponseWithData<KuickPayReceiptDetailsDTO>>(
                $"{_controller}/SaveKuickpayReceiptDetail", payload);
            return response;
        }
        public async Task<ApiResponseWithData<IBFTReceiptDetailDTO>> SaveIBFTReceiptDetail(IBFTReceiptPayload payload)
        {
            var response = await _mainApiClient.PostAsync<IBFTReceiptPayload, ApiResponseWithData<IBFTReceiptDetailDTO>>(
                $"{_controller}/SaveIBFTReceiptDetail", payload);
            return response;
        }

        //public async Task<ApiResponseWithData<TransactionReceiptPayload>> SaveTransactionReceiptDetail()
        //{
        //    return await _mainApiClient.GetAsync<ApiResponseWithData<TransactionReceiptPayload>>(
        //         $"{_controller}/SaveTransactionReceiptDetail"
        //     );
        //}
    }
}
