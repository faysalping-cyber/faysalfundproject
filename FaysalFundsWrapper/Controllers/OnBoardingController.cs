using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Models.AccountOpening;
using FaysalFundsWrapper.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace FaysalFundsWrapper.Controllers
{
    [Authorize]
    [Route("api/OnBoarding")]
    [ApiController]
    public class OnBoardingController : ControllerBase
    {
        private readonly IOnBoardingService _onBoardingService;
        private readonly IValidator<ContactDetail> _validator;
        public OnBoardingController(IOnBoardingService onBoardingService, IValidator<ContactDetail> validator)
        {
            _onBoardingService = onBoardingService;
            _validator = validator;
        }
        #region Post Requests
        //Basic Information
        [HttpPost("SaveFundSelection")]
        public async Task<IActionResult> SaveFundSelection(AccountSelectionRequestModel request)
        {
            request.ValidateAndThrow(new AccountSelectionValidation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveFundSelection(request);

            return Ok(result);
        }

        [HttpPost("SaveBasicInformationStep1")]
        public async Task<IActionResult> SaveBasicInformationStep1(BasicInformation1 request)
        {
            // Validate request
            request.ValidateAndThrow(new BasicInformation1Validation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveBasicInformationStep1(request);
            return Ok(result);
        }

        [HttpPost("SaveBasicInformationStep2")]
        public async Task<IActionResult> SaveBasicInformationStep2(BasicInformation2 request)
        {
            request.ValidateAndThrow(new BasicInformation2Validation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveBasicInformationStep2(request);
            return Ok(result);
        }

        [HttpPost("SaveContactDetail")]
        public async Task<IActionResult> SaveContactDetail(ContactDetail request)
        {
            await _validator.ValidateAndThrowAsync(request); 
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveContactDetail(request);
            return Ok(result);
        }


        [HttpPost("SaveNextToKinInformation")]
        public async Task<IActionResult> SaveNextToKinInformation(NextToKinInfo request)
        {
            request.ValidateAndThrow(new NextToKinInfoValidation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveNextToKinInformation(request);
            return Ok(result);
        }

        [HttpPost("SaveBankAccountInformation")]
        public async Task<IActionResult> SaveBankAccountInformation(BankDetails request)
        {
            request.ValidateAndThrow(new BankDetailsValidation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveBankAccountInformation(request);
            return Ok(result);
        }
        [HttpPost("SaveRegulatoryKYC_Step1")]
        public async Task<IActionResult> SaveRegulatoryKYC_Step1(RegulatoryKYC1 request)
        {
            request.ValidateAndThrow(new RegulatoryKYCStep1Validation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveRegulatoryKYC_Step1(request);
            return Ok(result);
        }
        [HttpPost("SaveRegulatoryKYC_Step2")]
        public async Task<IActionResult> SaveRegulatoryKYC_Step2(RegulatoryKYC2 request)
        {
            request.ValidateAndThrow(new RegulatoryKYCStep2Validation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveRegulatoryKYC_Step2(request);
            return Ok(result);
        }
        [HttpPost("SaveFATCA")]
        public async Task<IActionResult> SaveFATCA(FATCA request)
        {
            request.ValidateAndThrow(new FatcaValidation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveFATCA(request);
            return Ok(result);
        }
        [HttpPost("SaveCRS")]
        public async Task<IActionResult> SaveCRS(CRS request)
        {
            request.ValidateAndThrow(new CrsValidation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveCRS(request);
            return Ok(result);
        }
        [HttpPost("SaveRiskProfile")]
        public async Task<IActionResult> SaveRiskProfile(RiskProfile request)
        {
            request.ValidateAndThrow(new RiskProfileValidation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.SaveRiskProfile(request);
            return Ok(result);
        }
        [HttpPost("UploadDocuments")]
        public async Task<IActionResult> UploadDocuments(DocumentUpload request)
        {
            request.ValidateAndThrow(new UploadDocumentationValidation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.UploadDocuments(request);
            return Ok(result);
        }
        [HttpPost("OnboardingSubmit")]
        public async Task<IActionResult> PostOnboardinGProfile(OnboardingSubmitStatusDTO request)
        {
            request.ValidateAndThrow(new OnBoardigSubmitValidation());
            request.UserId = GetUserIdFromToken();
            var result = await _onBoardingService.PostOnboardingProfile(request);
            return Ok(result);
        }
        [HttpGet("CheckOnBoardingSteps")]
        public async Task<IActionResult> CheckOnBoardingSteps()
        {
            var result = await _onBoardingService.CheckOnboardingSteps(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });
            return Ok(result);
        }
        [HttpGet("OnBoardingStatus")]
        public async Task<IActionResult> GetOnBoardingStatus()
        {
            var result = await _onBoardingService.GetOnBoardingStatus(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });
            return Ok(result);
        }
        #endregion

        #region Get Requests
        //Basic Information
        [HttpGet("GetSelectedAccount")]
        public async Task<IActionResult> GetSelectedAccount()
        {
            var result = await _onBoardingService.GetSelectedAccount(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });
            return Ok(result);
        }
        [HttpGet("GetBasicInformationStep1")]
        public async Task<IActionResult> GetBasicInformationStep1()
        {
            var result = await _onBoardingService.GetBasicInformationStep1(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }
        [HttpGet("GetBasicInformationStep2")]
        public async Task<IActionResult> GetBasicInformationStep2()
        {
            var result = await _onBoardingService.GetBasicInformationStep2(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }

        [HttpGet("GetContactDetails")]
        public async Task<IActionResult> GetContactDetail()
        {
            var result = await _onBoardingService.GetContactDetail(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }

        [HttpGet("GetNextToKinInfo")]
        public async Task<IActionResult> GetNextToKinInfo()
        {
            var result = await _onBoardingService.GetNextToKinInfo(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }

        [HttpGet("GetBankDetails")]
        public async Task<IActionResult> GetBankDetails()
        {
            var result = await _onBoardingService.GetBankDetails(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }
        [HttpGet("GetRegulatoryKYC_Step1")]
        public async Task<IActionResult> GetRegulatoryKYC_Step1()
        {
            var result = await _onBoardingService.GetRegulatoryKYC_Step1(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }
        [HttpGet("GetRegulatoryKYC_Step2")]
        public async Task<IActionResult> GetRegulatoryKYC_Step2()
        {
            var result = await _onBoardingService.GetRegulatoryKYC_Step2(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }
        [HttpGet("GetCRS")]
        public async Task<IActionResult> GetCRS()
        {
            var result = await _onBoardingService.GetCRS(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }
        [HttpGet("GetFATCA")]
        public async Task<IActionResult> GetFATCA()
        {
            var result = await _onBoardingService.GetFATCA(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }
        [HttpGet("GetRiskProfileSavedValues")]
        public async Task<IActionResult> GetRiskProfileSavedValues()
        {
            var result = await _onBoardingService.GetRiskProfileSavedValues(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }
        [HttpGet("GetRiskProfileDropdowns")]
        public async Task<IActionResult> GetRiskProfileDropdowns()
        {
            var result = await _onBoardingService.GetRiskProfileDropdowns();

            return Ok(result);
        }
        [HttpGet("GetTransactionTypes")]
        public async Task<IActionResult> GetActiveFundsList()
        {
            var result = await _onBoardingService.GetActiveFundsList();

            return Ok(result);
        }
        [HttpGet("GetProfileUploadDocuments")]
        public async Task<IActionResult> GetProfileUploadDocuments()
        {
            var result = await _onBoardingService.GetProfileUploadDocuments(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }
      
        [HttpGet("GetRiskProfileScore")]
        public async Task<IActionResult> GetRiskProfile()
        {
            var result = await _onBoardingService.GetRiskProfile(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }
        [HttpGet("CompletedStepsCount")]
        public async Task<IActionResult> CompletedStepsCount()
        {
            var result = await _onBoardingService.CompletedStepsCount(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });

            return Ok(result);
        }
        #endregion

        private long GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return long.Parse(userIdClaim.Value);
        }
    }
}
