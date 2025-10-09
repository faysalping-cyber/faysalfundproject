using FaysalFunds.Application.DTOs;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace FaysalFunds.Application.Services
{
    public class AccountService
    {
        private readonly IUHSRepository _uHSRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly EncryptionService _encryptionService;
        private readonly UHSService _UHSService;
        private readonly LFDService _lFDService;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly ILoginAttemptRepository _loginAttemptRepository;
        private readonly IUserPasswordHistoryRepository _userPasswordHistoryRepository;
        private readonly double _lockMinute;
        public AccountService(EncryptionService encryptionService, IAccountRepository accountRepository, UHSService uHSService, LFDService lFDService, ILoginAttemptRepository loginAttemptRepository, IUHSRepository uHSRepository, /*TokenProviderService tokenProviderService,*/ IUserPasswordHistoryRepository userPasswordHistoryRepository, RefreshTokenService refreshTokenService, IOptions<Settings> options)
        {
            _encryptionService = encryptionService;
            _accountRepository = accountRepository;
            _UHSService = uHSService;
            _lFDService = lFDService;
            _loginAttemptRepository = loginAttemptRepository;
            _uHSRepository = uHSRepository;
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
            _refreshTokenService = refreshTokenService;
            _lockMinute = options.Value.lockMinutes;
        }

        public async Task<ApiResponseWithData<SignupResponseModel>> Signup(Signup signup)
        {
            await AreSignupParametersExists(signup.Email, signup.CNIC, signup.CountryCode, signup.PhoneNo);
            //var LFD = await _lFDService.Search(signup.CNIC);
            await _UHSService.CheckCustomer(new CheckCustomerRequestModel
            {
                CNIC = signup.CNIC,
                PhoneNo = signup.PhoneNo,
                Email = signup.Email
            });
            var hashedPassword = _encryptionService.HashPassword(signup.Password);
            var account = new Account
            {
                CNIC = signup.CNIC,
                COUNTRYCODE = signup.CountryCode,
                PHONE_NO = signup.PhoneNo,
                EMAIL = signup.Email.ToLower(),
                NAME = signup.Name,
                //LFD_FLAG = LFD.flag,
                PASSWORD = hashedPassword
            };
            var response = await _accountRepository.CreateAccount(account);
            if (response < 1)
            {
                throw new ApiException("Signup failed.");
            }
            var userId = await _accountRepository.GetUserIdByEmail(account.EMAIL);
            var signupResponseModel = new SignupResponseModel()
            {
                Email = account.EMAIL,
                UserId = userId
            };
            return ApiResponseWithData<SignupResponseModel>.SuccessResponse(signupResponseModel, "Signup successful");
        }

        public async Task<ApiResponseNoData> ChangePassword(ChangePasswordModel model)
        {

            var account = await _accountRepository.GetAccountByAccountId(model.UserId);
            if (model.OldPassword == model.NewPassword)
            {
                throw new ApiException("New password must be different from old password.");
            }
            var isOldPasswordCorrect = _encryptionService.VerifyPassword(account.PASSWORD, model.OldPassword);
            if (!isOldPasswordCorrect)
            {
                throw new ApiException("Old password is incorrect.");
            }

            var existsInPreviousThreePasswords = await _accountRepository.CheckAgainstLastThreePasswords(model.UserId, model.NewPassword);
            if (existsInPreviousThreePasswords)
            {
                throw new ApiException("Please don't use any of your last 3 passwords.");
            }

            //var hashedNewPassword = _encryptionService.HashPassword(model.NewPassword);
            await _accountRepository.SavePasswordToDb(model.UserId, model.NewPassword);

            return ApiResponseNoData.SuccessResponse("Password change successful.");
        }
        public async Task<ApiResponseWithData<LoginResponseModel>> Login(Login model)
        {
            var accountId = await _accountRepository.GetUserIdByEmail(model.Email);
            if (accountId < 1)
            {
                throw new ApiException("No Account associated with the given email.");
            }

            var attempt = await _loginAttemptRepository.GetLoginAttemptAsync(accountId);
            var account = await _accountRepository.GetAccountByAccountId(accountId);
            //if (account.OTP_IS_VERIFIED != 1)
            //{
            //    throw new ApiException("Please verify your OTP before logging in.");
            //}

            if (attempt != null && attempt.IS_LOCKED == 1 && attempt.LOCK_UNTIL > DateTime.UtcNow)
            {
                throw new ApiException($"Account locked. Try again after {_lockMinute} minutes.");
            }

            if (account == null || !_encryptionService.VerifyPassword(account.PASSWORD, model.Password))
            {
                await _loginAttemptRepository.RecordFailedAttemptAsync(accountId);
                throw new ApiException("Incorrect password.");
            }
            //LFD Check
            //await CHeckAndSaveUserScreening(accountId, account.CNIC);
            await _loginAttemptRepository.ResetAttemptsAsync(accountId);
            var refreshToken = await _refreshTokenService.SaveRefreshToken(account.EMAIL);

            var result = await _accountRepository.IsDeviceRegistered(model.RegisteredDeviceId, accountId);

            var responseModel = new LoginResponseModel()
            {
                UserId = accountId,
                NAME = account.NAME,
                Email = account.EMAIL,
                Cnic = account.CNIC,
                PhoneNo = account.PHONE_NO,
                CountryCode = account.COUNTRYCODE,
                IsDeviceRegistered = result == 2,
                AccessToken = "",
                RefreshToken = refreshToken.Data
            };

            return ApiResponseWithData<LoginResponseModel>.SuccessResponse(responseModel, "Login successful");
        }

        //public async Task<ApiResponseNoData> ResetPassword(ResetPasswordModel model)
        //{
        //    var isExists = await _accountRepository.IsCellCnicEmailSequenceExists(model.CellNo, model.CNIC, model.Email);
        //    if (isExists)
        //    {
        //        var accountId = await _accountRepository.GetUserIdByEmail(model.Email);
        //       var existsInPreviousThreePasswords = await _accountRepository.CheckAgainstLastThreePasswords(accountId, model.NewPassword);
        //        if (existsInPreviousThreePasswords)
        //            return ApiResponseNoData.FailureResponse("please don't use any previous passwords");
        //       await _accountRepository.SavePasswordToDb(accountId,model.NewPassword);
        //    }
        //    else
        //    {
        //        return ApiResponseNoData.FailureResponse("No Account found.");
        //    }
        //    return ApiResponseNoData.SuccessResponse("Password reset successful");
        //}
        public async Task CheckLFDUserScreeningEntryInDB(long userId)
        {
            var isExists = await _accountRepository.CheckLFDUserScreeningEntryInDB(userId);
            if (isExists)
                throw new ApiException("We regret to inform you that we are unable to proceed with your account opening request due to restrictions associated with your CNIC. Please contact (021) 111 329 725 for further details.", "Account Restricted");
        }
        public async Task<ApiResponseNoData> ResetPassword(ResetPasswordModel model)
        {

            long accountId = await _accountRepository.GetUserIdByEmail(model.Email);
            if (accountId < 1)
            {
                throw new ApiException("The given email is not associated with any account.");
            }
            var existsInPreviousThreePasswords = await _accountRepository.CheckAgainstLastThreePasswords(accountId, model.NewPassword);
            if (existsInPreviousThreePasswords)
                throw new ApiException("please don't use any previous passwords");
            await _accountRepository.SavePasswordToDb(accountId, model.NewPassword);
            return ApiResponseNoData.SuccessResponse("Password reset successful");
        }

        public async Task<ApiResponseNoData> SetPassword(long accountId, string PlainPassword)
        {
            var emailExists = await _accountRepository.GetAccountByAccountId(accountId);
            if (emailExists == null)
                throw new ApiException("Email not found.");
            await _accountRepository.SetPassword(PlainPassword, accountId);
            return ApiResponseNoData.SuccessResponse("Password is set successfully.");
        }

        public async Task UserDetailsVerification(string cnic, string email, string mobileNo)
        {
            await _uHSRepository.IsEmailAndMobileExistsAgainstCNIC(cnic, mobileNo, email);

        }

        public async Task<ApiResponseWithData<long>> GetUserIdByEmail(string email)
        {
            var id = await _accountRepository.GetUserIdByEmail(email);
            if (id == 0)
            {
                throw new ApiException("No account is associated with the given email address.");
            }
            return ApiResponseWithData<long>.SuccessResponse(id, "");
        }

        public async Task<long> GetUserIdByCNIC(string cnic)
        {
            var id = await _accountRepository.GetUserIdByCNIC(cnic);
            return id;
        }

        public async Task IsAccountExistAgainstUserId(long id)
        {
            var isExist = await _accountRepository.IsAccountExist(id);
            if (!isExist)
                throw new ApiException("No user found agaonst the given Id");
        }
        public async Task<ApiResponseNoData> UsersAppAccountVarification(UserDetailsVerificationModel model)
        {
            var isExists = await _accountRepository.IsCellCnicEmailSequenceExists(model.PhoneNo, model.CNIC, model.Email);
            if (isExists)
            {
                return ApiResponseNoData.SuccessResponse("A valid user.");
            }
            else
            {
                throw new ApiException("Not a valid user.");
            }
        }
        public async Task CHeckAndSaveUserScreening(long userId, string cnic)
        {
            var LFD = await _lFDService.Search(cnic);
            //var isExists = await _accountRepository.CheckLFDUserScreeningEntryInDB(userId);
            //if (LFD!=null)
            await _accountRepository.SaveLFDEntry(userId, LFD.flag);
        }

        public async Task CheckIfPhoneNoAndEmailExists(string cell, string email)
        {
            var exists = await _accountRepository.CheckIfPhoneNoAndEmailExists(cell, email);
            if (!exists)
            {
                throw new ApiException("No user found.");
            }
        }
        private async Task AreSignupParametersExists(string email, string cnic, string countryCode, string phoneNo)
        {
            bool isEmailExists = await _accountRepository.GetUserIdByEmail(email) > 0;
            bool isCnicExists = await _accountRepository.GetUserIdByCNIC(cnic) > 0;
            bool isPhoneNoExists = await _accountRepository.GetUserIdByCellNo(countryCode, phoneNo) > 0;

            if (isEmailExists)
            {
                throw new ApiException("Email already associated with an account.");
            }
            else if (isCnicExists)
            {
                throw new ApiException("CNIC already associated with an account.");
            }
            else if (isPhoneNoExists)
            {
                throw new ApiException("Phone no. already associated with an account.");
            }
        }
    }
}
