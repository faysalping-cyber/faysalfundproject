using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.DTOs.OTP;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Common.Enums;
using FaysalFunds.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace FaysalFunds.Application.Services
{
    public class OtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly EmailService _emailService;
        private readonly SmsService _smsService;
        private readonly AccountService _AccountService;
        private readonly Settings _settings;
        public OtpService(IOtpRepository otpRepository, IAccountRepository accountRepository, EmailService emailService, SmsService SmsService, IOptions<Settings> settings, AccountService accountService)
        {
            _otpRepository = otpRepository;
            _accountRepository = accountRepository;
            _emailService = emailService;
            _smsService = SmsService;
            _settings = settings.Value;
            _AccountService = accountService;
        }
        public async Task<ApiResponseNoData> ValidateOtpAsync(ValidateOtpModel request)
        {
            var otpResponse = new OtpValidationResult();
            var userId = await _accountRepository.GetUserIdByEmail(request.Email);

            var response = await _otpRepository.ValidateOtpAsync(userId, request.EmailOTP, request.CellOTP);
            if (response == OtpEnums.IsInValid)
            {
                return ApiResponseNoData.FailureResponse("Invalid OTP");
            };

            //if (response == OtpEnums.IsValid)
            //{
            //    await _accountRepository.MarkOtpVerifiedAsync(userId);
            //    return ApiResponseNoData.SuccessResponse("OTP verified successfully.");
            //}

            return ApiResponseNoData.SuccessResponse("OTP verified successfully.");
        }
        //public async Task<ApiResponseWithData<GenerateOtpResponseModel>> GenerateOtpAsync(string email, string mobile, byte isWhatsapp, byte SameOtp,string otpToken)
        //{
        //    var userId = await _accountRepository.GetUserIdByEmail(email);
        //    string emailOtp;
        //    string mobileOtp;
        //    int response;
        //    if (SameOtp ==1)
        //    {
        //        //string otp = GenerateSecureOtp();
        //        string otp = "1111";
        //        response = await _otpRepository.SaveOtpsInDb(email, otp, mobile, otp, isWhatsapp, userId, otpToken);
        //        emailOtp = mobileOtp = otp;
        //    }
        //    else
        //    {
        //        emailOtp = GenerateSecureOtp();
        //        mobileOtp = GenerateSecureOtp();

        //        //emailOtp = "1111";
        //        //mobileOtp = "2222";
        //        response = await _otpRepository.SaveOtpsInDb(email, emailOtp, mobile, mobileOtp, isWhatsapp, userId, otpToken);
        //    }
        //    if (response < 1)
        //    {
        //        return ApiResponseWithData<GenerateOtpResponseModel>.FailureResponse();
        //    }
        //    Console.WriteLine("Email OTP: " + emailOtp);
        //    Console.WriteLine("Mobile OTP: " + mobileOtp);

        //    // Send OTPs
        //    //await _emailService.SendOtpEmailAsync(email, emailOtp);
        //    if (isWhatsapp == 1)
        //    {
        //        // await _whatsappService.SendWhatsappAsync(mobile, $"Your WhatsApp OTP is {mobileOtp}. It expires in 5 minutes.");
        //    }
        //    else
        //    {
        //         await _smsService.SendSmsAsync(mobile, $"Your SMS OTP is {mobileOtp}. It expires in 5 minutes.");
        //    }
        //    GenerateOtpResponseModel model = new GenerateOtpResponseModel()
        //    {
        //        OtpToken = otpToken,
        //        OtpExpiry = Convert.ToInt32(_settings.OtpExpirationSeconds)
        //    };
        //    return ApiResponseWithData<GenerateOtpResponseModel>.SuccessResponse(model,"OTP sent successfully.");
        //}

        public async Task<ApiResponseWithData<GenerateOtpResponseModel>> GenerateOtpAsync(string email, string mobile, string CountryCode, byte isWhatsapp, byte SameOtp, string otpToken)
        {
            var userId = await _accountRepository.GetUserIdByEmail(email);
            string cleanedCountryCode = CountryCode.StartsWith("+") ? CountryCode.Substring(1) : CountryCode;
            string fullMobileNumber = cleanedCountryCode + mobile;
            mobile = fullMobileNumber;
            string emailOtp;
            string mobileOtp;
            int response;

            if (SameOtp == 1)
            {
                // Generate same OTP for both
                //string otp = GenerateSecureOtp();
                string otp = "1111";
                response = await _otpRepository.SaveOtpsInDb(email, otp, mobile, otp, isWhatsapp, userId, otpToken);
                emailOtp = mobileOtp = otp;
            }
            else
            {
                // Generate different OTPs
                //emailOtp = GenerateSecureOtp();
                //mobileOtp = GenerateSecureOtp();

                //response = await _otpRepository.SaveOtpsInDb(email, emailOtp, mobile, mobileOtp, isWhatsapp, userId, otpToken);
                string otp = "1111";
                response = await _otpRepository.SaveOtpsInDb(email, otp, mobile, otp, isWhatsapp, userId, otpToken);
                emailOtp = mobileOtp = otp;
            }

            if (response < 1)
            {
                return ApiResponseWithData<GenerateOtpResponseModel>.FailureResponse();
            }

            //Console.WriteLine("Email OTP: " + emailOtp);
            //Console.WriteLine("Mobile OTP: " + mobileOtp);

            // ✅ Send Email OTP
            await _emailService.SendOtpEmailAsync(email, emailOtp);

            // ✅ Send Mobile OTP (SMS or WhatsApp)
            if (isWhatsapp == 1)
            {
                // Uncomment when WhatsApp service is implemented
                // await _whatsappService.SendWhatsappAsync(mobile, $"Your WhatsApp OTP is {mobileOtp}. It expires in 5 minutes.");
            }
            else
            {
                await _smsService.SendSmsAsync(mobile, $"{mobileOtp}. is Your OTP for Faysal Funds Digital and is valid for 5 minutes. Do not share this with anyone. For assistance, call 111 329 725.");
            }

            GenerateOtpResponseModel model = new GenerateOtpResponseModel()
            {
                OtpToken = otpToken,
                OtpExpiry = Convert.ToInt32(_settings.OtpExpirationSeconds)
            };

            return ApiResponseWithData<GenerateOtpResponseModel>.SuccessResponse(model, "OTP sent successfully.");
        }


        private static string GenerateSecureOtp()
        {
            using var rng = RandomNumberGenerator.Create(); // Secure Random Generator
            var bytes = new byte[4]; // 4 bytes for enough randomness
            rng.GetBytes(bytes); // Fill the array with secure random numbers
            int otp = (int)(BitConverter.ToUInt32(bytes, 0) % 10000); // Ensure a 4-digit OTP
            return otp.ToString("D4"); // Format as a 4-digit string
        }

    

    }
}