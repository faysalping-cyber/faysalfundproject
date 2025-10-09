using FaysalFunds.Common;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FaysalFunds.Persistence.Repositories
{
    public class LoginAttemptRepository : BaseRepository<LoginAttempt>, ILoginAttemptRepository
    {
        private readonly DbSet<LoginAttempt> _loginAttemptSet;
        private readonly double _lockMinute;


        public LoginAttemptRepository(ApplicationDbContext dbContext, IOptions<Settings> options) : base(dbContext)
        {
            _loginAttemptSet = dbContext.Set<LoginAttempt>();
            _lockMinute = options.Value.lockMinutes;
        }

        public async Task<LoginAttempt?> GetLoginAttemptAsync(long accountId)
        {
            return await _loginAttemptSet.FirstOrDefaultAsync(x => x.ID == accountId);
        }

        public async Task RecordFailedAttemptAsync(long accountId)
        {
            var attempt = await GetLoginAttemptAsync(accountId);
            var now = DateTime.UtcNow;

            if (attempt == null)
            {
                attempt = new LoginAttempt
                {
                    ACCOUNTID = accountId,
                    FAILED_ATTEMPTS = 1,
                    LAST_ATTEMPT = now
                };
                await AddAsync(attempt);
            }
            else
            {
                // Check if the last attempt was within 5 minutes
                if (attempt.LAST_ATTEMPT.HasValue && (now - attempt.LAST_ATTEMPT.Value).TotalMinutes <= 5)
                {
                    attempt.FAILED_ATTEMPTS++;
                }
                else
                {
                    // Reset failed attempts if it's outside the 5-minute window
                    attempt.FAILED_ATTEMPTS = 1;
                }

                attempt.LAST_ATTEMPT = now;

                if (attempt.FAILED_ATTEMPTS >= 3)
                {
                    attempt.IS_LOCKED = 1;
                    attempt.LOCK_UNTIL = now.AddMinutes(_lockMinute);
                }
            }
            await SaveChangesAsync();
        }

        public async Task ResetAttemptsAsync(long accountId)
        {
            var attempt = await GetLoginAttemptAsync(accountId);
            if (attempt != null)
            {
                attempt.FAILED_ATTEMPTS = 0;
                attempt.IS_LOCKED = 0;
                attempt.LOCK_UNTIL = null;
                await SaveChangesAsync();
            }
        }
    }
}
