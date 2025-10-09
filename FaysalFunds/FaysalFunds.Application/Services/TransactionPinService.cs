using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.DTOs.AccountOpening;
using FaysalFunds.Application.DTOs.AccountOpening.AccountSelection;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;


namespace FaysalFunds.Application.Services
{
    public class TransactionPinService
    {
        private readonly ITransactionPinRepository _transactionPinRepository;
        private readonly EncryptionService _encryptionService;
        private readonly AccountService _accountservice;
        private readonly IAccountRepository _accountRepository;

        public TransactionPinService(ITransactionPinRepository transactionPinRepository, EncryptionService encryptionService, IAccountRepository accountRepository, AccountService accountservice)
        {
            _transactionPinRepository = transactionPinRepository;
            _encryptionService = encryptionService;
            _accountservice = accountservice;
            _accountRepository = accountRepository;
        }

        public async Task<ApiResponseNoData> GenerateTransactionPin(GenerateTPinRequest model)
        {

            if (string.IsNullOrWhiteSpace(model.Pin) || model.Pin.Length != 4)
            {
                throw new ApiException("T-PIN must be exactly 4 digits.");

            }
            if (!model.Pin.All(char.IsDigit))
            {
                throw new ApiException("T-PIN must contain only digits.");

            }
            var existingPin = await _transactionPinRepository.GetTpinByAccountOpeningId(model.AccountOpeningId);
            if (existingPin != null)
            {
                throw new ApiException("T-PIN already exists for this account.");
            }
            // Hash Pin and create entity
            var hashedPin = _encryptionService.PinHasher(model.Pin);
            var newPin = new TransactionPin
            {
                ACCOUNTOPENING_ID = model.AccountOpeningId,
                PINHASH = hashedPin,
                CREATEDON = DateTime.UtcNow
            };

            var savedPin = await _transactionPinRepository.GenerateTransactionPin(newPin);

            if (!savedPin)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }

        public async Task<ApiResponseNoData> VerifyTransactionPin(VerifyTpinRequest model)
        {
            var savedPin = await _transactionPinRepository.GetTpinByAccountOpeningId(model.AccountOpeningId);
            if (savedPin == null)
                throw new ApiException("T-PIN you have entered is incorrect, Kindly enter correct T-pin");

            // Check if account is already locked
            if (savedPin.ISLOCKED == 1)
                throw new ApiException("Account is locked due to multiple failed T-PIN attempts.");

            bool isMatch = _encryptionService.VerifyPin(model.Pin, savedPin.PINHASH);

            if (!isMatch)
            {
                savedPin.FAILEDATTEMPTS = (savedPin.FAILEDATTEMPTS ?? 0) + 1;

                // Lock account after 3 failed attempts
                if (savedPin.FAILEDATTEMPTS >= 3)
                {
                    savedPin.ISLOCKED = 1;
                    savedPin.LOCKEDON = DateTime.UtcNow;
                    await _transactionPinRepository.UpdateTransactionPin(savedPin);

                    throw new ApiException("Account is locked due to multiple failed T-PIN attempts.");
                }

                await _transactionPinRepository.UpdateTransactionPin(savedPin);
                throw new ApiException("T-PIN does not match.");
            }

            // Reset failed attempts on success
            savedPin.FAILEDATTEMPTS = 0;
            await _transactionPinRepository.UpdateTransactionPin(savedPin);

            return ApiResponseNoData.SuccessResponse("T-PIN verified successfully.");
        }

        //Cnage TransactionPin
        public async Task<ApiResponseNoData> ChangeTransactionPin(ChangeTPinRequest model)
        {
            
            var existingPin = await _transactionPinRepository.GetTpinByAccountOpeningId(model.AccountOpeningId);
            if (existingPin == null)

                throw new ApiException("T-PIN not found.");

            bool isOldPinCorrect = _encryptionService.VerifyPin(model.OldPin, existingPin.PINHASH);
            if (!isOldPinCorrect)
                throw new ApiException("Old T-PIN does not match.");

            existingPin.PINHASH = _encryptionService.PinHasher(model.NewPin);
            existingPin.UPDATEDON = DateTime.UtcNow;

            var updated = await _transactionPinRepository.UpdateTransactionPin(existingPin);
            return updated
                ? ApiResponseNoData.SuccessResponse("T-PIN changed successfully.")
                : ApiResponseNoData.FailureResponse("Failed to change T-PIN.");
        }

        //forgot T-Pin
        public async Task<ApiResponseNoData> ForgotTransactionPin(ForgotTPinRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.NewPin) || model.NewPin.Length != 4 || !model.NewPin.All(char.IsDigit))
            throw new ApiException("T-PIN must be exactly 4 digits.");

            var existing = await _transactionPinRepository.GetTpinByAccountOpeningId(model.AccountOpeningId);
            if (existing == null)
            throw new ApiException("T-PIN record not found for this account.");

            bool isSamePin = _encryptionService.VerifyPin(model.NewPin, existing.PINHASH);
            if (isSamePin)
            throw new ApiException("This T-PIN is already set.");

            existing.PINHASH = _encryptionService.PinHasher(model.NewPin);
            existing.UPDATEDON = DateTime.UtcNow;
            existing.ISLOCKED = 0;
            existing.FAILEDATTEMPTS = 0;
            existing.LOCKEDON = null;
            var updated = await _transactionPinRepository.UpdateTransactionPin(existing);
            return updated
                ? ApiResponseNoData.SuccessResponse("T-PIN updated successfully.")
                : ApiResponseNoData.FailureResponse("Failed to update T-PIN.");
        }

        //public async Task<ApiResponseNoData> VerifyUserBeforeForgotTpin(VerifyUserBeforeForgotTpin model)
        //{

        //    var account = await _accountRepository.GetAccountByAccountId(model.UserId);

        //    var updated = await _transactionPinRepository.UpdateTransactionPin(existing);
        //    return updated
        //        ? ApiResponseNoData.SuccessResponse("T-PIN updated successfully.")
        //        : ApiResponseNoData.FailureResponse("Failed to update T-PIN.");
        //}

        public async Task<ApiResponseNoData> VerifyUserBeforeForgotTpin(VerifyUserBeforeForgotTpin model)
        {
            var account = await _accountRepository.GetAccountByAccountId(model.UserId);

            if (account == null)
                throw new ApiException("Account not found.");

            if (account.CNIC != model.Cnic)
                throw new ApiException("CNIC does not match.");

            if (account.EMAIL !=model.Email)
                throw new ApiException("Email does not match.");

            if (account.COUNTRYCODE != model.CountryCode)
                throw new ApiException("Country code does not match.");

            if (account.PHONE_NO !=model.PhoneNumber)
                throw new ApiException("Phone number does not match.");

            return ApiResponseNoData.SuccessResponse("User verification successful.");
        }

    }
}



