using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Models.AccountOpening;
using FaysalFundsWrapper.Models.AccountOpening;
using System.Security.Claims;

namespace FaysalFundsWrapper.Services
{
    public class OnBoardingService : IOnBoardingService
    {
        private readonly string _controller = "AccountOpening";
        private readonly IMainApiClient _mainApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OnBoardingService(IMainApiClient mainApiClient, IHttpContextAccessor httpContextAccessor)
        {
            _mainApiClient = mainApiClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponseNoData> SaveFundSelection(AccountSelectionRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<AccountSelectionRequestModel, ApiResponseNoData>($"{_controller}/SaveFundSelection", request);
            return response;
        }
        public async Task<ApiResponseNoData> SaveBasicInformationStep1(BasicInformation1 request)
        {
            var response = await _mainApiClient.PostAsync<BasicInformation1,ApiResponseNoData>($"{_controller}/SaveBasicInformationStep1", request);
            return response;
        }
        public async Task<ApiResponseNoData> SaveBasicInformationStep2(BasicInformation2 request)
        {
            var response = await _mainApiClient.PostAsync<BasicInformation2,ApiResponseNoData>($"{_controller}/SaveBasicInformationStep2", request);
            return response;
        }
        public async Task<ApiResponseNoData> SaveContactDetail(ContactDetail request)
        {
            var response = await _mainApiClient.PostAsync<ContactDetail,ApiResponseNoData>($"{_controller}/SaveContactDetail", request);
            return response;
        }
        public async Task<ApiResponseNoData> SaveNextToKinInformation(NextToKinInfo request)
        {
            var response = await _mainApiClient.PostAsync<NextToKinInfo,ApiResponseNoData>($"{_controller}/SaveNextToKinInformation", request);
            return response;
        }
        public async Task<ApiResponseNoData> SaveBankAccountInformation(BankDetails request)
        {
            var response = await _mainApiClient.PostAsync<BankDetails,ApiResponseNoData>($"{_controller}/SaveBankAccountInformation", request);
            return response;
        }
        public async Task<ApiResponseNoData> SaveRegulatoryKYC_Step1(RegulatoryKYC1 request)
        {
            var response = await _mainApiClient.PostAsync<RegulatoryKYC1, ApiResponseNoData>($"{_controller}/SaveRegulatoryKYC_Step1", request);
            return response;
        }

        public async Task<ApiResponseNoData> SaveRegulatoryKYC_Step2(RegulatoryKYC2 request)
        {
            var response = await _mainApiClient.PostAsync<RegulatoryKYC2, ApiResponseNoData>($"{_controller}/SaveRegulatoryKYC_Step2", request);
            return response;
        }

        public async Task<ApiResponseNoData> SaveFATCA(FATCA request)
        {
            var response = await _mainApiClient.PostAsync<FATCA, ApiResponseNoData>($"{_controller}/SaveFATCA", request);
            return response;
        }

        public async Task<ApiResponseNoData> SaveCRS(CRS request)
        {
            var response = await _mainApiClient.PostAsync<CRS, ApiResponseNoData>($"{_controller}/SaveCRS", request);
            return response;
        }

        public async Task<ApiResponseNoData> SaveRiskProfile(RiskProfile request)
        {
            var response = await _mainApiClient.PostAsync<RiskProfile, ApiResponseNoData>($"{_controller}/SaveRiskProfileDropdownValues", request);
            return response;
        }

        public async Task<ApiResponseNoData> UploadDocuments(DocumentUpload request)
        {
            var response = await _mainApiClient.PostAsync<DocumentUpload, ApiResponseNoData>($"{_controller}/UploadProfileDocuments", request);
            return response;
        }
        public async Task<ApiResponseNoData> PostOnboardingProfile(OnboardingSubmitStatusDTO request)
        {
            var response = await _mainApiClient.PostAsync<OnboardingSubmitStatusDTO, ApiResponseNoData>($"{_controller}/OnboardingSubmit", request);
            return response;
        }
        //GET

        public async Task<ApiResponseWithData<AccountSelectionResponseModel>> GetSelectedAccount(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<AccountSelectionResponseModel>>($"{_controller}/GetSelectedAccount", request);
            return response;
        }
        public async Task<ApiResponseWithData<BasicInformation1GetModel>> GetBasicInformationStep1(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<BasicInformation1GetModel>>($"{_controller}/GetBasicInformationStep1", request);
            return response;
        }
        public async Task<ApiResponseWithData<BasicInformation2GetModel>> GetBasicInformationStep2(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<BasicInformation2GetModel>>($"{_controller}/GetBasicInformationStep2", request);
            return response;
        }

        public async Task<ApiResponseWithData<ContactDetailGetModel>> GetContactDetail(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<ContactDetailGetModel>>($"{_controller}/GetContactDetail", request);
            return response;
        }

        public async Task<ApiResponseWithData<NextToKinInfoGetModel>> GetNextToKinInfo(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<NextToKinInfoGetModel>>($"{_controller}/GetNextToKinInfo", request);
            return response;
        }

        public async Task<ApiResponseWithData<BankDetailsGetModel>> GetBankDetails(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<BankDetailsGetModel>>($"{_controller}/GetBankDetails", request);
            return response;
        }

        public async Task<ApiResponseWithData<RegulatoryKYC_Step1GetModel>> GetRegulatoryKYC_Step1(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<RegulatoryKYC_Step1GetModel>>($"{_controller}/GetRegulatoryKYC_Step1", request);
            return response;
        }

        public async Task<ApiResponseWithData<RegulatoryKYC_Step2GetModel>> GetRegulatoryKYC_Step2(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<RegulatoryKYC_Step2GetModel>>($"{_controller}/GetRegulatoryKYC_Step2", request);
            return response;
        }

        public async Task<ApiResponseWithData<FATCA_GetModel>> GetFATCA(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<FATCA_GetModel>>($"{_controller}/GetFATCA", request);
            return response;
        }

        public async Task<ApiResponseWithData<CRS_GetModel>> GetCRS(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<CRS_GetModel>>($"{_controller}/GetCRS", request);
            return response;
        }

        public async Task<ApiResponseWithData<RiskProfileGetModel>> GetRiskProfileSavedValues(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<RiskProfileGetModel>>($"{_controller}/GetRiskProfileSavedValues", request);
            return response;
        }

        public async Task<ApiResponseWithData<RiskProfileDropDownDTO>> GetRiskProfileDropdowns()
        {
            var response = await _mainApiClient.GetAsync<ApiResponseWithData<RiskProfileDropDownDTO>>($"{_controller}/GetRiskProfileDropdowns");
            return response;
        }

        public async Task<ApiResponseWithData<ActiveFunds>> GetActiveFundsList()
        {
            var response = await _mainApiClient.GetAsync<ApiResponseWithData<ActiveFunds>>($"{_controller}/GetActiveFundsList");
            return response;
        }

        public async Task<ApiResponseWithData<UploadDocumentsGetModel>> GetProfileUploadDocuments(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<UploadDocumentsGetModel>>($"{_controller}/GetProfileUploadDocuments", request);
            return response;
        }
      
        public async Task<ApiResponseWithData<RiskScoreGetModel>> GetRiskProfile(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<RiskScoreGetModel>>($"{_controller}/GetRiskProfile", request);
            return response;
        }

        public async Task<ApiResponseWithData<CompletedStepsCountModel>> CompletedStepsCount(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<CompletedStepsCountModel>>($"{_controller}/CompletedStepsCount", request);
            return response;
        }
        public async Task<ApiResponseWithData<OnboardingStatusResponse>> GetOnBoardingStatus(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<OnboardingStatusResponse>>($"{_controller}/GetOnBoardingStatus", request);
            return response;
        }

        public async Task<ApiResponseWithData<MissingStepsResponseModel>> CheckOnboardingSteps(OnBoardingRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<OnBoardingRequestModel, ApiResponseWithData<MissingStepsResponseModel>>($"{_controller}/CheckOnboardingSteps", request);
            return response;
        }
        public long GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new Exception("User ID claim not found.");

            return long.Parse(userIdClaim.Value);
        }

        public async Task<int> GetSavedAccountSelection()
        {
            var request = new GetAccountTypeRequestModel
            {
                UserId = GetUserId()
            };
            var response = await _mainApiClient.PostAsync<GetAccountTypeRequestModel, ApiResponseWithData<GetAccountTypeResponseModel>>($"{_controller}/GetSavedAccountSelection", request);
            return response.Data.AccountType;
        }
    }
}