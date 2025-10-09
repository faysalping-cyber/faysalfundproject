using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Models.AccountOpening;
using FaysalFundsWrapper.Models.Dashboard;
using FaysalFundsWrapper.Services;
using FaysalFundsWrapper.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Transactions;
namespace FaysalFundsWrapper.Controllers
{
    [Authorize]
    [Route("api/dashboard")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashhboardServices _dashhboardServicesService;

        public DashBoardController(IDashhboardServices dashhboardServicesService)
        {
            _dashhboardServicesService = dashhboardServicesService;
        }
        [HttpPost("AddQuickAccessMenu")]
        public async Task<IActionResult> GenerateTPin(QuickAccesModel request)
        {
   
            var result = await _dashhboardServicesService.AddQuickAccessMenu(request);
            return Ok(result);
        }


        [HttpGet("GetActiveQuickAccessMenu")]
        public async Task<IActionResult> GetActiveQuickAccessMenu()
        {
            var result = await _dashhboardServicesService.GetAllQuickAccessMenu();
            return Ok(result);
        }
        [HttpPost("GetUserQuickAccessMenus")]
        public async Task<IActionResult> GetUserQuickAccessMenus()
        {
            long userId = GetUserIdFromToken();
            var validator = new QuickAccessValidation.GetUserQuickAccessMenusValidator();
            validator.ValidateAndThrow(userId);

            var result = await _dashhboardServicesService.GetUserQuickAccessMenus(userId);
            return Ok(result);
        }



        [HttpPost("AddUserQuickAccess")]
        public async Task<IActionResult> AddUserQuickAccess(UserQuickAccessDto request)
        {
            var validator = new QuickAccessValidation.AddUserQuickAccessValidator();
            validator.ValidateAndThrow(request);
            request.USERID = GetUserIdFromToken();
            var result = await _dashhboardServicesService.AddUserQuickAccess(request);
            return Ok(result);
        }
        [HttpDelete("RemoveUserQuickAccess")]
        public async Task<IActionResult> RemoveUserQuickAccess(UserQuickAccessDto request)
        {
            var validator = new QuickAccessValidation.RemoveUserQuickAccessValidator();
            validator.ValidateAndThrow(request);
            request.USERID = GetUserIdFromToken();
            var result = await _dashhboardServicesService.RemoveUserQuickAccess(request);
            return Ok(result);
        }

        [HttpPost("CheckBalance")]
        public async Task<IActionResult> CheckBalance(CheckBalanceRequest request)
        {
            //var validator = new QuickAccessValidation.AddUserQuickAccessValidator();
            //validator.ValidateAndThrow(request);
            CheckBalanceRequestModel model = new()
            {
                Folio = request.FolioNo,
                Cnic = User.FindFirst(CustomClaimTypes.CNIC)?.Value,
                PhoneNo = User.FindFirst(ClaimTypes.MobilePhone)?.Value
            };
            var result = await _dashhboardServicesService.CheckBalance(model);
            return Ok(result);
        }

        [HttpPost("CheckProfit")]
        public async Task<IActionResult> CheckProfit(CheckProfitRequest request)
        {
            //var validator = new QuickAccessValidation.AddUserQuickAccessValidator();
            //validator.ValidateAndThrow(request);
            CheckProfitRequestModel model = new()
            {
                FolioNo = request.FolioNo,
                CNIC = User.FindFirst(CustomClaimTypes.CNIC)?.Value,
                PhoneNo = User.FindFirst(ClaimTypes.MobilePhone)?.Value
            };
            var result = await _dashhboardServicesService.CheckProfit(model);
            return Ok(result);
        }

        [HttpGet("GetCustomerAvailableFolio")]
        public async Task<IActionResult> GetCustomerAvailableFolio()
        {
            //var validator = new QuickAccessValidation.AddUserQuickAccessValidator();
            //validator.ValidateAndThrow(request);
            CustomerAvailableFolioRequest model = new()
            {
                CNIC = User.FindFirst(CustomClaimTypes.CNIC)?.Value,
                PhoneNo = User.FindFirst(ClaimTypes.MobilePhone)?.Value
            };
            var result = await _dashhboardServicesService.GetCustomerAvailableFolio(model);
            return Ok(result);
        }
        private long GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return long.Parse(userIdClaim.Value);
        }
        //Dashboard
        [HttpGet("GetInvestmentFunds")]
        public async Task<IActionResult> GetInvestmentFunds()
        {
            
            var result = await _dashhboardServicesService.GetAllFunds(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });
            return Ok(result);
        }

        [HttpGet("GetKuickPayCharges")]
        public async Task<IActionResult> GetKuickPayChanges()
        {
            
            var result = await _dashhboardServicesService.GetAllKuickPayCharges(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });
            return Ok(result);
        }


        [HttpGet("GetTransactionTypes")]
        public async Task<IActionResult> GetAllTransactionTypes()
        {
            
            var result = await _dashhboardServicesService.GetAllTransactionTypes(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });
            return Ok(result);
        }

        [HttpGet("TransactionFeature")]
        public async Task<IActionResult> GetAllTransactionFeature()
        {
            
            var result = await _dashhboardServicesService.GetAllTransactionFeature(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });
            return Ok(result);
        }

        [HttpGet("GetInvestmentInstructions")]
        public async Task<IActionResult> GetInvestmentInstructions()
        {
           
            var result = await _dashhboardServicesService.GetInvestmentInstructions(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });
            return Ok(result);
        }
        [HttpPost("CheckRaastTransactionAllowed")]
        public async Task<IActionResult> CheckRaastTransactionAllowed()
        {

            var result = await _dashhboardServicesService.CheckRaastTransactionAllowed(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });
            return Ok(result);
        }
        [HttpPost("GetViewTransactionLimit")]
        public async Task<IActionResult> GetViewTransactionLimit()
        {
         
            var result = await _dashhboardServicesService.GetViewTransactionLimit(new OnBoardingRequestModel { UserId = GetUserIdFromToken() });
            return Ok(result);
        }
        [HttpPost("GetTransactionFeatureById")]
        public async Task<IActionResult> GetTransactionFeatureById([FromBody] TransactionID request)
        {
            long userId = GetUserIdFromToken();
            request.UserId = userId;
            var result = await _dashhboardServicesService.GetTransactionFeatureById(request);
            return Ok(result);
        }

        [HttpPost("check-permissions")]
        public async Task<IActionResult> GetFeaturePermissions([FromBody] FeaturePermissionRequestDTO request)
        {
            long userId = GetUserIdFromToken();
            request.UserId = userId;
            var result = await _dashhboardServicesService.GetFeaturePermissions(request);
            return Ok(result);
        }

        //[HttpPost("CalculateKuickPay")]
        //public async Task<IActionResult> CalculateKuickPay(CalculateKuickPayLoad payload)
        //{
        //    long userId = GetUserIdFromToken();
        //    payload.UserId = userId;
        //    var result = await _dashhboardServicesService.CalculateKuickPay(payload);
        //    return Ok(result);
        //}
        [HttpPost("CalculateKuickPay")]
        public async Task<ApiResponseWithData<CalculateKuickPayDTO>> CalculateKuickPay([FromBody] CalculateKuickPayLoad payload)
        {
            long userId = GetUserIdFromToken();
            payload.UserId = userId;

            var result = await _dashhboardServicesService.CalculateKuickPay(payload);
            return result;
        }

        [HttpPost("GenerateKuickPayID")]
        public async Task<ApiResponseWithData<KuickPayFinalResponse>> GenerateKuickPayID([FromBody] GenerateKuickPayIdDTO payload)
        {
            long userId = GetUserIdFromToken();
            payload.UserId = userId;
            var result = await _dashhboardServicesService.GenerateKuickPayID(payload);
            return result;
        }
        [HttpPost("SaveKuickpayReceiptDetail")]
        public async Task<ApiResponseWithData<KuickPayReceiptDetailsDTO>> SaveKuickpayReceiptDetail([FromBody] KuickPayReceiptPayload payload)
        {
            long userId = GetUserIdFromToken();
            payload.UserId = userId;
            var result = await _dashhboardServicesService.SaveKuickpayReceiptDetail(payload);
            return result;
        }
        [HttpPost("SaveIBFTReceiptDetail")]
        public async Task<ApiResponseWithData<IBFTReceiptDetailDTO>> SaveIBFTReceiptDetail([FromBody] IBFTReceiptPayload payload)
        {
            long userId = GetUserIdFromToken();
            payload.UserId = userId;
            var result = await _dashhboardServicesService.SaveIBFTReceiptDetail(payload);
            return result;
        }
    }
}
