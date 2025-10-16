using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.DTOs.AccountOpening;
using FaysalFunds.Application.DTOs.ExternalAPI;
using FaysalFunds.Application.DTOs.TransactionAllowedDTO;
using FaysalFunds.Application.Services;
using FaysalFunds.Domain.DTOs.ExternalAPI;
using FaysalFunds.Infrastructure.ExternalService;
using Microsoft.AspNetCore.Mvc;

namespace FaysalFunds.API.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly DashhboardServices _dashhboardServices;
        private KuickPayServices _KuickPayServices;
        private readonly FamlInternalService _famlInternalService;
        private readonly UHSService _UHSService;

        private readonly JWTBlackListService _jWTBlackListService;

        public DashBoardController(DashhboardServices dashhboardServices, KuickPayServices KuickPayServices, JWTBlackListService jWTBlackListService, DropdownService dropdownService, FamlInternalService famlInternalService, UHSService uHSService)
        {
            _dashhboardServices = dashhboardServices;
            _jWTBlackListService = jWTBlackListService;
            _KuickPayServices = KuickPayServices;
            _famlInternalService = famlInternalService;
            _UHSService = uHSService;
        }

        #region Add Requests
        [HttpPost("AddQuickAccessMenu")]
        public async Task<IActionResult> AddQuickAccessMenu(QuickAccesMenuDto request)
        {
            var result = await _dashhboardServices.AddQuickAccessMenu(request);
            return Ok(result);
        }

        //Combined code
        [HttpPost("GetUserQuickAccessMenus")]
        public async Task<IActionResult> GetUserQuickAccessMenus(AccountOpeningRequestModel request)

        {
            var result = await _dashhboardServices.GetUserQuickAccessDetails(request.UserId);
            return Ok(result);
        }

        [HttpPost("AddUserQuickAccess")]
        public async Task<IActionResult> AddUserQuickAccess([FromBody] UserQuickAccessDto request)
        {
            var result = await _dashhboardServices.SaveUserQuickAccess(request.USERID, request.QUICKACCESSID);
            return Ok(result);
        }

        #endregion
        //get
        #region Get Requests

        [HttpGet("GetActiveQuickAccessMenu")]
        public async Task<IActionResult> GetAllQuickAccessMenu()
        {
            var result = await _dashhboardServices.GetAllQuickAccessMenus();
            return Ok(result);
        }
        //
        [HttpGet("GetInvestmentFunds")]
        public async Task<IActionResult> GetInvestmentFunds()
        {
            var result = await _KuickPayServices.GetAllFunds();
            return Ok(result);
        }

        [HttpGet("GetKuickPayCharges")]
        public async Task<IActionResult> GetKuickPayChanges()
        {
            var result = await _KuickPayServices.GetAllKuickPayCharges();
            return Ok(result);
        }
     

        [HttpGet("GetTransactionTypes")]
        public async Task<IActionResult> GetAllTransactionTypes()
        {
            var result = await _KuickPayServices.GetAllTransactionTypes();
            return Ok(result);
        }

        [HttpGet("TransactionFeature")]
        public async Task<IActionResult> GetAllTransaconFeature()
        {
            var result = await _KuickPayServices.GetAllTransaconFeature();
            return Ok(result);
        }
        
        [HttpGet("GetInvestmentInstructions")]
        public async Task<IActionResult> GetInvestmentInstructions()
        {
            var result = await _KuickPayServices.GetInvestmentInstructions();
            return Ok(result);
        }
        [HttpPost("CheckRaastTransactionAllowed")]
        public async Task<IActionResult> CheckRaastTransactionAllowed(AccountOpeningRequestModel request)
        {
            var result = await _KuickPayServices.RaastAllowedorNot(request);
            return Ok(result);
        }


        [HttpPost("GetViewTransactionLimit")]
        public async Task<IActionResult> GetActiveAccountType(AccountOpeningRequestModel model)
        {
            var result = await _KuickPayServices.GetActiveFundById(model.UserId);
            return Ok(result);
        }
        [HttpPost("GetTransaconFeatureById")]
        public async Task<IActionResult> GetTransactionFeatureById(TransactionID request)
        {
            var result = await _KuickPayServices.GetTransactionFeatureById(request);
            return Ok(result);
        }

        [HttpPost("check-permissions")]
        public async Task<IActionResult> GetFeaturePermissions([FromBody] FeaturePermissionRequestDTO request)
        {
            var result = await _KuickPayServices.GetFeaturePermissions(request);
            return Ok(result);
        }
        [HttpPost("CalculateKuickPay")]
        public async Task<IActionResult> CalculateKuickPay(CalculateKuickPayLoad payload)
        {
            var result = await _KuickPayServices.CalculateKuickPay(payload);
            return Ok(result);
        }
        [HttpPost("SaveKuickpayReceiptDetail")]
        public async Task<IActionResult> SaveTransactionReceiptDetail(KuickPayReceiptPayload payload)
        {
            var result = await _KuickPayServices.SaveKuickpayReceiptDetail(payload);
            return Ok(result);
        }
        [HttpPost("SaveIBFTReceiptDetail")]
        public async Task<IActionResult> SaveIBFTReceiptDetail(IBFTReceiptPayload payload)
        {
            var result = await _KuickPayServices.SaveIBFTReceiptDetail(payload);
            return Ok(result);
        }
        //
        #endregion


        [HttpPost("RemoveUserQuickAccess")]
        public async Task<IActionResult> RemoveUserQuickAccess([FromBody] RemoveUserQuickAccessDto request)
        {
            var result = await _dashhboardServices.RemoveUserQuickAccess(request.USERID, request.QUICKACCESSID);
            return Ok(result);
        }
        [HttpPost("GetCustomerAvailableFolio")]
        public async Task<IActionResult> GetCustomerAvailableFolio([FromBody] CustomerAvailableFolioRequestModel request)
        {
            var result = await _UHSService.GetCustomerAvailableFolio(request.PhoneNo, request.CNIC);
            return Ok(result);
        }

        [HttpPost("CheckBalance")]
        public async Task<IActionResult> CheckBalance([FromBody] CheckBalanceRequestModel request)
        {
            var result = await _famlInternalService.CheckCustomerBalance(request);
            return Ok(result);
        }

        [HttpPost("CheckProfit")]
        public async Task<IActionResult> CheckProfit([FromBody] ProfitRequestModel request)
        {
            var result = await _famlInternalService.CheckCustomerProfit(request);
            return Ok(result);
        }

        [HttpPost("GenerateKuickPayId")]
        public async Task<IActionResult> GenerateKuickPayId([FromBody] GenerateKuickPayIdDTO request)
        {
            var result = await _famlInternalService.GenerateKuickPayId(request);
            return Ok(result);
        }
        //[HttpPost("AlreadyInvestedFunds")]
        //public async Task<IActionResult> SelectinvestedFunds(AccountOpeningRequestModel request)
        //{
        //    var result = await _KuickPayServices.SelectinvestedFunds(request);
        //    return Ok(result);
        //}
    }
}
