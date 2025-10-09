using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace FaysalFunds.Persistence.Repositories
{
    public class ProfileScoreRepository : BaseRepository<ProfileScore>, IProfileScoreRepository
    {
        private readonly DbSet<ProfileScore> _profileScoreSet;

        public ProfileScoreRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _profileScoreSet = dbContext.Set<ProfileScore>();
        }
        public async Task<bool> Upsert(ProfileScore profileScore)
        {
            
            var existingScore = await _profileScoreSet
        .FirstOrDefaultAsync(ps => ps.ACCOUNTOPENING_ID == profileScore.ACCOUNTOPENING_ID);
            var riskLevel = GetRiskLevel(profileScore.TOTAL_SCORE);
            if (existingScore != null)
            {
                // Update existing
                existingScore.TOTAL_SCORE = profileScore.TOTAL_SCORE;
                existingScore.RISKLEVEL = riskLevel;
                Update(existingScore);
            }
            else
            {
                // Insert new
                profileScore.RISKLEVEL = riskLevel;
                await AddAsync(profileScore);
            }
            return await SaveChangesAsync()>0;

        }

        public async Task<ProfileScore> GetRiskProfile(long userId)
        {
            var result =await _profileScoreSet
    .Where(rps => rps.AccountOpening.ACCOUNTID == userId)
    .Select(rps => new ProfileScore
    {
       TOTAL_SCORE= rps.TOTAL_SCORE,
        RISKLEVEL = rps.RISKLEVEL,
        ACCOUNTOPENING_ID = 0,
        ID = 0
    }).FirstOrDefaultAsync();

            return result;
        }
        private string GetRiskLevel(int score)
        {
            return score switch
            {
                > 50 => "High",
                >= 44 => "Medium",
                >= 36 => "Moderate",
                _ => "Low"
            };
        }


    }
}
