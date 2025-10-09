using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace FaysalFunds.Persistence.Repositories
{
    public class JWTBlackListRepository : BaseRepository<JwtBlacklist>, IJWTBlackListRepository
    {
        public JWTBlackListRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<int> BlackListToken(string token, DateTime expirationDate)
        {
            var blacklistedToken = new JwtBlacklist
            {
                TOKEN = token,
                EXPIRATIONDATE = expirationDate
            };
            await AddAsync(blacklistedToken);
            var response = await SaveChangesAsync();

            return response;
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _dbSet.CountAsync(t => t.TOKEN == token) > 0;
        }
    }
}