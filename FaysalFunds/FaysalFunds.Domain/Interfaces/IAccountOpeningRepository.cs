using FaysalFunds.Domain.Entities;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IAccountOpeningRepository
    {
        Task<bool> AddAccountSelection(AccountOpening model);
        Task<int> BasicInfoCompletedSteps(long userId);
        Task<int> CalculateRiskProfileScore(long accountOpeningId);
        Task<bool> DoesAnEntryExistsAgainstUserId(long userId);
        Task<long> GetAccountOpeningIdByUserId(long userId);
        Task<AccountOpening> GetAccountSelection(long userId);
        Task<AccountOpening> GetBankDetails(long userId);
        Task<AccountOpening> GetBasicInformationStep1(long userId);
        Task<AccountOpening> GetBasicInformationStep2(long userId);
        Task<AccountOpening> GetContactDetail(long userId);
        Task<AccountOpening> GetCRS(long userId);
        Task<AccountOpening> GetFATCA(long userId);
        Task<AccountOpening> GetNextToKinInformation(long userId);
        Task<AccountOpening> GetRegulatoryKYC1(long userId);
        Task<AccountOpening> GetRegulatoryKYC2(long userId);
        Task<AccountOpening> GetRiskProfileSavedValues(long userId);
        Task<AccountOpening> GetUploadedDocuments(long userId);
        //Task<AccountOpening> GetUploadedDigitalDocuments(long userId);
        Task<bool> IsBasicInfoEnabled(long userId);
        Task<bool> IsPersonalDetailsEnabled(long userId);
        Task<bool> IsRegulatoryKYCEnabled(long userId);
        Task<bool> IsRiskProfileEnabled(long userId);
        Task<bool> IsUploadDocumentEnabled(long userId);
        Task<int> OnBoardingStatus(long userId);
        Task<bool> PostOnboardingProfile(AccountOpening model);

        Task<int> PersonalDetailsCompletedSteps(long userId);
        Task<int> RegulatoryKYCCompletedSteps(long userId);
        Task<bool> SaveBankDetails(AccountOpening model);
        Task<bool> SaveBasicInformationStep1(AccountOpening model);
        Task<bool> SaveBasicInformationStep2(AccountOpening model);
        Task<bool> SaveContactDetail(AccountOpening model);
        Task<bool> SaveCRS(AccountOpening model);
        Task<bool> SaveFATCA(AccountOpening model);
        Task<bool> SaveNextToKinInformation(AccountOpening model);
        Task<bool> SaveRegulatoryKYC1(AccountOpening model);
        Task<bool> SaveRegulatoryKYC2(AccountOpening model);
        Task<bool> SaveRiskProfileDropdownValues(AccountOpening model);
        Task<List<(int Id, string Title, int TotalSteps, int CompletedSteps, int IsEnabled)>> StepsCounts(long userId);
        Task<bool> UploadProfileDocuments(AccountOpening model);
        Task<AccountOpening?>GetAccountOpening(long userId);


    }
}