using FaysalFunds.Domain.Entities;

namespace FaysalFunds.Domain.Interfaces
{
    public interface ILoginAttemptRepository
    {
        Task<LoginAttempt> GetLoginAttemptAsync(long accountId);
        Task RecordFailedAttemptAsync(long accountId);
        Task ResetAttemptsAsync(long accountId);
    }
}
