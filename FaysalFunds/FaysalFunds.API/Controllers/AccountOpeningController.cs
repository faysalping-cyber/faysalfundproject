using FaysalFunds.API.Models;
using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.DTOs.AccountOpening;
using FaysalFunds.Application.DTOs.AccountOpening.RegulatoryKYC;
using FaysalFunds.Application.Services;
using FaysalFunds.Application.Services.IServices;
using FaysalFunds.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Pkcs;
using System.Text.Json.Nodes;

namespace FaysalFunds.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountOpeningController : ControllerBase
    {
        private readonly AccountOpeningService _accountOpeningService;
        private readonly RiskProfileService _riskProfileService;
        private readonly FamlFundsService _famlFundsService;
        private readonly ProfileScoreService  _profileScoreService;
        private readonly TransactionPinService _transactionPinService;
        private readonly IEmailService _emailService;
        public AccountOpeningController(AccountOpeningService accountOpeningService, RiskProfileService riskProfileService, FamlFundsService famlFundsService, ProfileScoreService profileScoreService, TransactionPinService transactionPinService, IEmailService emailService
            )
        {
            _accountOpeningService = accountOpeningService;
            _riskProfileService = riskProfileService;
            _famlFundsService = famlFundsService;
            _profileScoreService = profileScoreService;
            _transactionPinService = transactionPinService;
            _emailService = emailService;
        }

        #region Post Requests

        [HttpPost("SaveFundSelection")]
        public async Task<IActionResult> AddAccount(AccountSelectionRequestModel request)
        {
            var result = await _accountOpeningService.AddAccountSelection(request);

            return Ok(result);
        }

        [HttpPost("SaveBasicInformationStep1")]
        public async Task<IActionResult> SaveBasicInformationStep1(BasicInformation1 request)
        {
            var result = await _accountOpeningService.SaveBasicInformationStep1(request);
            return Ok(result);
        }

        [HttpPost("SaveBasicInformationStep2")]
        public async Task<IActionResult> SaveBasicInformationStep2(BasicInformation2 request)
        {
            var result = await _accountOpeningService.SaveBasicInformationStep2(request);
            return Ok(result);
        }

        [HttpPost("SaveContactDetail")]
        public async Task<IActionResult> SaveContactDetail(ContactDetail request)
        {
            var result = await _accountOpeningService.SaveContactDetail(request);
            return Ok(result);
        }

        [HttpPost("SaveNextToKinInformation")]
        public async Task<IActionResult> SaveNextToKinInformation(NextToKinInfo request)
        {
            var result = await _accountOpeningService.SaveNextToKinInformation(request);
            return Ok(result);
        }

        [HttpPost("SaveBankAccountInformation")]
        public async Task<IActionResult> SaveBankAccountInformation(BankDetails request)
        {
            var result = await _accountOpeningService.SaveBankAccountInformation(request);
            return Ok(result);
        }

        [HttpPost("SaveRegulatoryKYC_Step1")]
        public async Task<IActionResult> SaveRegulatoryKYC_Step1(RegulatoryKYC1 request)
        {
            var result = await _accountOpeningService.SaveRegulatoryKYC_Step1(request);
            return Ok(result);
        }
        [HttpPost("SaveRegulatoryKYC_Step2")]
        public async Task<IActionResult> SaveRegulatoryKYC_Step2(RegulatoryKYC2 request)
        {
            var result = await _accountOpeningService.SaveRegulatoryKYC_Step2(request);
            return Ok(result);
        }
        [HttpPost("SaveFATCA")]
        public async Task<IActionResult> SaveFATCA(FATCA request)
        {
            var result = await _accountOpeningService.SaveFATCA(request);
            return Ok(result);
        }
        [HttpPost("SaveCRS")]
        public async Task<IActionResult> SaveCRS(CRS request)
        {
            var result = await _accountOpeningService.SaveCRS(request);
            return Ok(result);
        }
        [HttpPost("SaveRiskProfileDropdownValues")]
        public async Task<IActionResult> SaveRiskProfileDropdownValues(RiskProfile request)
        {
            var result = await _accountOpeningService.SaveRiskProfileDropdownValues(request);
            return Ok(result);
        }

        [HttpPost("UploadProfileDocuments")]
        public async Task<IActionResult> UploadProfileDocuments(UploadDocuments request)
        {
            var result = await _accountOpeningService.UploadProfileDocuments(request);
            return Ok(result);
        }
        [HttpPost("OnBoardingSubmit")]
        public async Task<IActionResult> PostOnboardingProfile(OnboardingSubmitStatusDTO request)
        {
            var result = await _accountOpeningService.PostOnboardingProfile(request);
            return Ok(result);
        }
        [HttpPost("sendEmail")]
        //public async Task<IActionResult> SendEmail([FromBody] SendEmailDTO request)
        //{
        //    try
        //    {
        //        await _emailService.SendEmailAsync(
        //            request.ToEmail,
        //            request.Subject,
        //            request.Body,
        //            request.IsHtml,
        //            request.IncludeExcelAttachment // Pass the flag to include Excel attachment
        //        );
        //        return Ok("Email sent successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error sending email: {ex.Message}");
        //    }
        //}
        #endregion

        #region Get Requests

        [HttpGet("GetActiveFundsList")]
        public async Task<IActionResult> GetActiveFundsList()
        {
            var result = await _famlFundsService.GetActiveFunds();

            return Ok(result);
        }

        [HttpPost("GetSelectedAccount")]
        public async Task<IActionResult> GetSelectedAccount(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetAccountSelection(request);
            return Ok(result);
        }
        [HttpPost("GetBasicInformationStep1")]
        public async Task<IActionResult> GetBasicInformationStep1(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetBasicInformationStep1(request);

            return Ok(result);
        }
        [HttpPost("GetBasicInformationStep2")]
        public async Task<IActionResult> GetBasicInformationStep2(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetBasicInformationStep2(request);

            return Ok(result);
        }

        [HttpPost("GetContactDetail")]
        public async Task<IActionResult> GetContactDetail(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetContactDetail(request);

            return Ok(result);
        }

        [HttpPost("GetNextToKinInfo")]
        public async Task<IActionResult> GetNextToKinInfo(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetNextToKinInfo(request);

            return Ok(result);
        }

        [HttpPost("GetBankDetails")]
        public async Task<IActionResult> GetBankDetails(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetBankDetails(request);

            return Ok(result);
        }
        [HttpPost("GetRegulatoryKYC_Step1")]
        public async Task<IActionResult> GetRegulatoryKYC_Step1(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetRegulatoryKYC_Step1(request);

            return Ok(result);
        }
        [HttpPost("GetRegulatoryKYC_Step2")]
        public async Task<IActionResult> GetRegulatoryKYC_Step2(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetRegulatoryKYC_Step2(request);

            return Ok(result);
        }
        [HttpPost("GetFATCA")]
        public async Task<IActionResult> GetFATCA(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetFATCA(request);

            return Ok(result);
        }
        [HttpPost("GetCRS")]
        public async Task<IActionResult> GetCRS(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetCRS(request);

            return Ok(result);
        }
        [HttpPost("GetRiskProfileSavedValues")]
        public async Task<IActionResult> GetRiskProfileSavedValues(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetRiskProfileSavedValues(request);

            return Ok(result);
        }
        [HttpGet("GetRiskProfileDropdowns")]
        public async Task<IActionResult> GetRiskProfileDropdowns()
        {
            var result = await _riskProfileService.GetRiskProfileDropdownsAsync();

            return Ok(result);
        }
        [HttpPost("GetRiskProfile")]
        public async Task<IActionResult> GetRiskProfile(AccountOpeningRequestModel requestModel)
        {
            var result = await _profileScoreService.GetRiskProfile(requestModel.UserId);

            return Ok(result);
        }

        [HttpPost("GetProfileUploadDocuments")]
        public async Task<IActionResult> GetProfileUploadDocuments(AccountOpeningRequestModel requestModel)
        {
            var result = await _accountOpeningService.GetProfileUploadDocuments(requestModel.UserId);

            return Ok(result);
        }
        [HttpPost("GetOnBoardingStatus")]
        public async Task<IActionResult> GetOnBoardingStatus([FromBody] AccountOpeningRequestModel requestModel)
        {
            
            var result = await _accountOpeningService.OnBoardingStatusAccounttype(requestModel.UserId);

            return Ok(result);
        }
        [HttpPost("CheckOnboardingSteps")]
        public async Task<IActionResult> CheckOnboardingSteps(AccountOpeningRequestModel requestModel)
        {

            var result = await _accountOpeningService.GetMissingOnboardingSteps(requestModel.UserId);

            return Ok(result);
        }

        [HttpPost("CompletedStepsCount")]
        public async Task<IActionResult> CompletedStepsCount(AccountOpeningRequestModel requestModel)
        {
            var result = await _accountOpeningService.StepsCounts(requestModel.UserId);

            return Ok(result);
        }
        #endregion
        [HttpPost("GenerateTPin")]
        public async Task<IActionResult> GenerateTPin(GenerateTPinRequest request)
        {
            //long accountOpeningId = long.Parse(User.FindFirst("AccountOpeningId").Value);

            var result = await _transactionPinService.GenerateTransactionPin(request);
            return Ok(result);
        }

        [HttpPost("VerifyTPin")]
        public async Task<IActionResult> VerifyTPin(VerifyTpinRequest request)
        {
            var result = await _transactionPinService.VerifyTransactionPin(request);
            return Ok(result);
        }
        [HttpPost("ChangeTPin")]
        public async Task<IActionResult> ChangeTPin(ChangeTPinRequest request)
        {
            var result = await _transactionPinService.ChangeTransactionPin(request);
            return Ok(result);
        }

        [HttpPost("ForgotTPin")]
        public async Task<IActionResult> ForgotTPin(ForgotTPinRequest request)
        {
            var result = await _transactionPinService.ForgotTransactionPin(request);
            return Ok(result);
        }
        [HttpPost("VerifyUserBeforeTpin")]
        public async Task<IActionResult> VerifyUserBeforeForgotTpin(VerifyUserBeforeForgotTpin request)
        {
            var result = await _transactionPinService.VerifyUserBeforeForgotTpin(request);
            return Ok(result);
        }

        [HttpPost("GetAccountOpeningIdByUserId")]
        public async Task<IActionResult> GetAccountOpeningIdByUserId(AccountOpeningRequestModel request)
        {
            var result = await _accountOpeningService.GetAccountOpeningIdByUserIdPublic(request.UserId);
            return Ok(result);
        }
        [HttpPost("GetSavedAccountSelection")]
        public async Task<IActionResult> GetSavedAccountSelection(GetAccountTypeRequestModel request)
        {
            var result = await _accountOpeningService.GetSavedAccountSelection(request.UserId);
            return Ok(result);
        }
    }
}