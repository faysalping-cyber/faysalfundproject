using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Validations;
using Microsoft.AspNetCore.Mvc;

namespace FaysalFundsWrapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly IOtpService _otpService;

        public OTPController(IOtpService otpService)
        {
            _otpService = otpService;
        }
        [HttpPost("GenerateOTP")]
        public async Task<IActionResult> GenerateOTP([FromBody] GenerateOTPModel request)
        {
            request.ValidateAndThrow(new GenerateOTPValidation());
            var result = await _otpService.GenerateOTP(request);

            return Ok(result);
        }
        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ValidateOTP([FromBody] ValidateOTPModel request)
        {
            request.ValidateAndThrow(new ValidateOTPValidation());
            var result = await _otpService.ValidateOTP(request);
            return Ok(result);
        }
    }
}
