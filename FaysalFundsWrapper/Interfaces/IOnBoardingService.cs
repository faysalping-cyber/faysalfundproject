using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Models.AccountOpening;

namespace FaysalFundsWrapper.Interfaces
{
    public interface IOnBoardingService
    {
 
        Task<ApiResponseWithData<AccountSelectionResponseModel>> GetSelectedAccount(OnBoardingRequestModel request);
        Task<ApiResponseWithData<BasicInformation1GetModel>> GetBasicInformationStep1(OnBoardingRequestModel request);
        Task<ApiResponseWithData<BasicInformation2GetModel>> GetBasicInformationStep2(OnBoardingRequestModel request);
        Task<ApiResponseWithData<ContactDetailGetModel>> GetContactDetail(OnBoardingRequestModel request);
        Task<ApiResponseWithData<NextToKinInfoGetModel>> GetNextToKinInfo(OnBoardingRequestModel request);
        Task<ApiResponseWithData<BankDetailsGetModel>> GetBankDetails(OnBoardingRequestModel request);
        Task<ApiResponseWithData<RegulatoryKYC_Step1GetModel>> GetRegulatoryKYC_Step1(OnBoardingRequestModel request);
        Task<ApiResponseWithData<RegulatoryKYC_Step2GetModel>> GetRegulatoryKYC_Step2(OnBoardingRequestModel request);
        Task<ApiResponseWithData<FATCA_GetModel>> GetFATCA(OnBoardingRequestModel request);
        Task<ApiResponseWithData<CRS_GetModel>> GetCRS(OnBoardingRequestModel request);
        Task<ApiResponseWithData<RiskProfileGetModel>> GetRiskProfileSavedValues(OnBoardingRequestModel request);
        Task<ApiResponseWithData<RiskProfileDropDownDTO>> GetRiskProfileDropdowns();
        Task<ApiResponseWithData<ActiveFunds>> GetActiveFundsList();
        Task<ApiResponseWithData<UploadDocumentsGetModel>> GetProfileUploadDocuments(OnBoardingRequestModel request);
        Task<ApiResponseWithData<RiskScoreGetModel>> GetRiskProfile(OnBoardingRequestModel request);
        Task<ApiResponseWithData<CompletedStepsCountModel>> CompletedStepsCount(OnBoardingRequestModel request);
        Task<ApiResponseNoData> SaveFundSelection(AccountSelectionRequestModel request);
        Task<ApiResponseNoData> SaveBasicInformationStep1(BasicInformation1 request);
        Task<ApiResponseNoData> SaveBasicInformationStep2(BasicInformation2 request);
        Task<ApiResponseNoData> SaveContactDetail(ContactDetail request);
        Task<ApiResponseNoData> SaveNextToKinInformation(NextToKinInfo request);
        Task<ApiResponseNoData> SaveBankAccountInformation(BankDetails request);
        Task<ApiResponseNoData> SaveRegulatoryKYC_Step1(RegulatoryKYC1 request);
        Task<ApiResponseNoData> SaveRegulatoryKYC_Step2(RegulatoryKYC2 request);
        Task<ApiResponseNoData> SaveFATCA(FATCA request);
        Task<ApiResponseNoData> SaveCRS(CRS request);
        Task<ApiResponseNoData> SaveRiskProfile(RiskProfile request);
        Task<ApiResponseNoData> UploadDocuments(DocumentUpload request);
        Task<ApiResponseNoData> PostOnboardingProfile(OnboardingSubmitStatusDTO request);
        Task<ApiResponseWithData<OnboardingStatusResponse>> GetOnBoardingStatus(OnBoardingRequestModel request);
        Task<ApiResponseWithData<MissingStepsResponseModel>> CheckOnboardingSteps(OnBoardingRequestModel request);

        Task<int> GetSavedAccountSelection();
    }
}
