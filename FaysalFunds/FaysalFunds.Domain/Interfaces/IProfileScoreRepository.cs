using FaysalFunds.Domain.Entities;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IProfileScoreRepository
    {
        Task<ProfileScore> GetRiskProfile(long userId);
        Task<bool> Upsert(ProfileScore profileScore);
    }
}
