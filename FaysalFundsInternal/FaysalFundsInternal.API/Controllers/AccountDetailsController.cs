using FaysalFundsInternal.Application.DTOs;
using FaysalFundsInternal.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaysalFundsInternal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountDetailsController : ControllerBase
    {
        private readonly UhrService _uhrService;
        private  readonly AccountStatementService _accountStatementService;
        private readonly KuickPayService _kuickPayService;
        public AccountDetailsController(UhrService uhrService, AccountStatementService accountStatementService, KuickPayService kuickPayService)
        {
            _uhrService = uhrService;
            _accountStatementService = accountStatementService;
            _kuickPayService = kuickPayService;
        }

        [HttpPost("CheckBalance")]
        public async Task<IActionResult> CheckBalance(CheckBalanceRequestModel request)
        {
            var result = await _uhrService.CheckBalance(request.Cnic, request.PhoneNo, request.Folio);
            return Ok(result);
        }

        [HttpPost("Profit")]
        public async Task<IActionResult> Profit(ProfitRequestmodel request)
        {
            var result = await _accountStatementService.Profit(request.FolioNo, request.IsFiscal);
            return Ok(result);
        }

        [HttpPost("GetSumOfProfit")]
        public async Task<IActionResult> GetSumOfProfit(MobileAppProfitRequestModel request)
        {
            var result = await _accountStatementService.Profit(request.CNIC,request.PhoneNo,request.FolioNo);
            return Ok(result);
        }
        [HttpPost("GenerateKuickPayID")]
        public async Task<IActionResult> GenerateKuickPayID(GenerateKuickPayIdDTO request)
        {
            var result = await _kuickPayService.GenerateKuickPayID(request);
            return Ok(result);
        }
    }
}
