using FaysalFunds.Common;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace FaysalFunds.Persistence.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        private readonly DbSet<Account> _accountSet;
        private readonly EncryptionService _encryptionService;
        public AccountRepository(ApplicationDbContext dbContext, EncryptionService encryptionService) : base(dbContext)
        {
            _accountSet = dbContext.Set<Account>(); // Initialize DbSet
            _encryptionService = encryptionService;
        }

        public async Task<int> CreateAccount(Account account)
        {
            await AddAsync(account);
            var response = await SaveChangesAsync();
            return response;
        }
        public async Task<bool> IsAccountExist(long accountId)
        {
            return await _accountSet.CountAsync(e => e.ID == accountId) > 0;
        }
        public async Task<long> GetUserIdByEmail(string email)
        {
            var id = await _accountSet.Where(e => e.EMAIL.ToLower() == email.ToLower()).Select(e => e.ID).FirstOrDefaultAsync();
            return id;
        }
        public async Task<long> GetUserIdByCNIC(string cnic)
        {
            var id = await _accountSet.Where(e => e.CNIC.ToLower() == cnic.ToLower()).Select(e => e.ID).FirstOrDefaultAsync();
            return id;
        }
        public async Task<Account?> GetAccountByAccountId(long accountId)
        {
            return await _accountSet
         .Include(u => u.USERPASSWORDHISTORY).AsNoTracking() // Ensure history is loaded
         .FirstOrDefaultAsync(u => u.ID == accountId);
        }
        public async Task<int> IsDeviceRegistered(string deviceId, long accountId)
        {
            return await _accountSet.CountAsync(e => e.REGISTERED_DEVICE_ID == deviceId && e.ID == accountId) > 0 ? 1 : 0;
        }
        public async Task<Account?> GetAccountAndPassHistoryByAccountId(long accountId)
        {
            return await _accountSet
                .Include(u => u.USERPASSWORDHISTORY)
                .FirstOrDefaultAsync(u => u.ID == accountId);
        }
        //public async Task<bool> ResetPassword(long accountId, string newPassword)
        //{
        //    // Fetch user with password history
        //    var user = await _context.Accounts
        //        .Include(u => u.USERPASSWORDHISTORY)
        //        .FirstOrDefaultAsync(u => u.ID == accountId);
        //    // Initialize password hasher
        //    //var passwordHasher = new PasswordHasher<User>();

        //    // Check against last 3 passwords
        //    var last3Passwords = user.USERPASSWORDHISTORY
        //        .OrderByDescending(p => p.CREATEDON)
        //        .Take(3)
        //        .ToList();

        //    foreach (var oldPassword in last3Passwords)
        //    {
        //        var verificationResult = _encryptionService.VerifyPassword(oldPassword.HASHEDPASSWORD, newPassword);
        //        if (verificationResult)
        //            throw new ApiException(ApiErrorCodes.Forbidden, "please don't use any previous passwords");
        //    }

        //    // Save current password to history BEFORE updating
        //    _context.UserPasswordHistories.Add(new UserPasswordHistory
        //    {
        //        ACCOUNTID = user.ID,
        //        HASHEDPASSWORD = user.PASSWORD, // old password
        //        CREATEDON = DateTime.UtcNow
        //    });

        //    // Hash and update new password
        //    user.PASSWORD = _encryptionService.HashPassword(newPassword);

        //    // Keep only the latest 3 in history
        //    var oldPasswordsToRemove = user.USERPASSWORDHISTORY
        //        .OrderByDescending(p => p.CREATEDON)
        //        .Skip(3)
        //        .ToList();

        //    if (oldPasswordsToRemove.Any())
        //        _context.UserPasswordHistories.RemoveRange(oldPasswordsToRemove);

        //    await _context.SaveChangesAsync();
        //    return true;
        //}
        public async Task<bool> SavePasswordToDb(long accountId, string newPassword)
        {
            var user = await _context.Accounts
                .Include(u => u.USERPASSWORDHISTORY)
                .FirstOrDefaultAsync(u => u.ID == accountId);
            // Save current password to history BEFORE updating
            _context.UserPasswordHistories.Add(new UserPasswordHistory
            {
                ACCOUNTID = user.ID,
                HASHEDPASSWORD = user.PASSWORD, // old password
                CREATEDON = DateTime.UtcNow
            });

            // Hash and update new password
            user.PASSWORD = _encryptionService.HashPassword(newPassword);

            // Keep only the latest 3 in history
            var oldPasswordsToRemove = user.USERPASSWORDHISTORY
                .OrderByDescending(p => p.CREATEDON)
                .Skip(3)
                .ToList();

            if (oldPasswordsToRemove.Any())
                _context.UserPasswordHistories.RemoveRange(oldPasswordsToRemove);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CheckAgainstLastThreePasswords(long accountId, string newPassword)
        {
            var user = await _context.Accounts
                .Include(u => u.USERPASSWORDHISTORY)
                .FirstOrDefaultAsync(u => u.ID == accountId);
            // Initialize password hasher
            //var passwordHasher = new PasswordHasher<User>();

            // Check against last 3 passwords
            var last3Passwords = user.USERPASSWORDHISTORY
                .OrderByDescending(p => p.CREATEDON)
                .Take(3)
                .ToList();

            foreach (var oldPassword in last3Passwords)
            {
                var verificationResult = _encryptionService.VerifyPassword(oldPassword.HASHEDPASSWORD, newPassword);
                if (verificationResult)
                    return true;
            }
            return false;
        }

        public async Task<bool> SetPassword(string password, long accountId)
        {
            // Fetch the account
            var user = await _accountSet
                .FirstOrDefaultAsync(u => u.ID == accountId);

            // Hash and set the new password
            user.PASSWORD = _encryptionService.HashPassword(password);

            // Save to password history
            _context.Add(new UserPasswordHistory
            {
                ACCOUNTID = user.ID,
                HASHEDPASSWORD = user.PASSWORD,
                CREATEDON = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> IsCellCnicEmailSequenceExists(string cell, string cnic, string email)
        {
            var count = await _accountSet.CountAsync(e => e.PHONE_NO == cell && e.CNIC == cnic && e.EMAIL == email);
            if (count > 0)
            {
                return true;

            }
            return false;
        }
        public async Task<long> GetUserIdByCellNo(string countryCode,string cellNo)
        {
            var id = await _accountSet.Where(e => e.PHONE_NO == cellNo && e.COUNTRYCODE == countryCode).Select(e => e.ID).FirstOrDefaultAsync();
            return id;
        }
        public async Task SaveLFDEntry(long userId,string lfdFlag)
        {
            var user = new Account { ID = userId};
            _context.Attach(user);

            // Modify only the LFD_FLAG property
            user.LFD_FLAG = lfdFlag;
            _context.Entry(user).Property(u => u.LFD_FLAG).IsModified = true;
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckLFDUserScreeningEntryInDB(long userId)
        {
            string? lfdEntry = await _accountSet.Where(e => e.ID == userId).Select(e => e.LFD_FLAG).FirstOrDefaultAsync();
            if (!String.IsNullOrEmpty(lfdEntry))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> CheckIfPhoneNoAndEmailExists(string cell, string email)
        {
            var count = await _accountSet.CountAsync(e => e.PHONE_NO == cell && e.EMAIL == email);
            if (count > 0)
            {
                return true;

            }
            return false;
        }

        public async Task<bool> MarkOtpVerifiedAsync(long userId)
        {
            var account = await _accountSet.FirstOrDefaultAsync(a => a.ID == userId);
            if (account != null)
            {
                account.OTP_IS_VERIFIED = 1;
                await SaveChangesAsync();
                return true; 
            }
            return false; 
        }


    }
}
