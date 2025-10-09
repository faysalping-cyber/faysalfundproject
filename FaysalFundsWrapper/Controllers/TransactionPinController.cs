using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FaysalFundsWrapper.Controllers
{
    [Authorize]
    [Route("api/TransactionPin")]
    [ApiController]
    public class TransactionPinController : ControllerBase
    {
        private readonly ITransactionPinService _transactionPinService;

        public TransactionPinController(ITransactionPinService transactionPinService)
        {
            _transactionPinService = transactionPinService;
        }

        [HttpPost("GenerateTPin")]
        public async Task<IActionResult> GenerateTPin(GenerateTPinRequest request)
        {
            var validator = new TPinValidation.GenerateTPinRequestValidator();
            validator.ValidateAndThrow(request);
            request.AccountOpeningId = GetUserIdFromToken();
            var result = await _transactionPinService.GenerateTransactionPin(request);
            return Ok(result);
        }

        [HttpPost("VerifyTPin")]
        public async Task<IActionResult> VerifyTPin(VerifyTpinRequest request)
        {
            var validator = new TPinValidation.VerifyTPinRequestValidator();
            validator.ValidateAndThrow(request);
            request.AccountOpeningId = GetUserIdFromToken();
            var result = await _transactionPinService.VerifyTransactionPin(request);
            return Ok(result);
        }
        [HttpPost("ChangeTPin")]
        public async Task<IActionResult> ChangeTPin(ChangeTPinRequest request)
        {
            var validator = new TPinValidation.ChangeTPinRequestValidator();
            validator.ValidateAndThrow(request);
            request.AccountOpeningId = GetUserIdFromToken();
            var result = await _transactionPinService.ChangeTransactionPin(request);
            return Ok(result);
        }

        [HttpPost("ForgotTPin")]
        public async Task<IActionResult> ForgotTPin(ForgotTPinRequest request)
        {
            var validator = new TPinValidation.ForgotTPinRequestValidator();
            validator.ValidateAndThrow(request);
            request.AccountOpeningId = GetUserIdFromToken();

            var result = await _transactionPinService.ForgotTransactionPin(request);
            return Ok(result);
        }
   
        [HttpPost("VerifyUserBeforeTpin")]
        public async Task<IActionResult> VerifyUserBeforeForgotTpin([FromBody] VerifyUserBeforeForgotTpin request)
        {
            var validator = new VerifyUserBeforeForgotTpinValidator();
            validator.ValidateAndThrow(request);
            request.UserId = GetUserIdFromToken();
            var result = await _transactionPinService.VerifyUserBeforeForgotTpin(request);

            return Ok(result);
        }

        private long GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return long.Parse(userIdClaim.Value);
        }
    }
}
