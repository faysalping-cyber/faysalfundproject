using FaysalFunds.Domain.Entities;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> CheckAgainstLastThreePasswords(long accountId, string newPassword);
        Task<bool> CheckIfPhoneNoAndEmailExists(string cell, string email);
        Task<bool> CheckLFDUserScreeningEntryInDB(long userId);
        Task<int> CreateAccount(Account account);
        Task<Account?> GetAccountAndPassHistoryByAccountId(long accountId);
        Task<Account?> GetAccountByAccountId(long accountId);
        Task<long> GetUserIdByCellNo(string countryCode, string cellNo);
        Task<long> GetUserIdByCNIC(string cnic);
        Task<long> GetUserIdByEmail(string email);
        Task<bool> IsAccountExist(long accountId);
        Task<bool> IsCellCnicEmailSequenceExists(string cell, string cnic, string email);
        Task<int> IsDeviceRegistered(string deviceId, long accountId);
        Task SaveLFDEntry(long userId, string lfdFlag);

        //Task<bool> ResetPassword(long accountId, string newPassword);
        Task<bool> SavePasswordToDb(long accountId, string newPassword);
        Task<bool> SetPassword(string password, long accountId);

        Task<bool> MarkOtpVerifiedAsync(long userId);

    }
}
