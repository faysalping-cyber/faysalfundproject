using FaysalFunds.API.Models;
using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.Services;
using FaysalFunds.Common.APIException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FaysalFunds.API.Controllers
{
    [Route("api/OTP")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly OtpService _otpService;

        public OtpController(OtpService otpService)
        {
            _otpService = otpService;
        }
        [HttpPost("GenerateOtp")]
        public async Task<IActionResult> GenerateOtp(GenerateOtpModel model)
        {
            byte isWhatsApp =  model.IsWhatsapp ? (byte)1  : (byte)0;
            byte sameOtp = model.SameOtp ? (byte)1 : (byte)0;
            var response =await _otpService.GenerateOtpAsync(model.Email, model.Mobile,model.CountryCode, isWhatsApp, sameOtp,model.OtpToken);
            return Ok(response);
        }

       
        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ValidateOtp(ValidateOtpModel model)
        {
            var response = await _otpService.ValidateOtpAsync(model);
            return Ok(response);
        }
    }
}
