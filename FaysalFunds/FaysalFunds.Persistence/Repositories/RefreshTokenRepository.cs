using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace FaysalFunds.Persistence.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly DbSet<RefreshToken> _refreshToken;
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            _refreshToken = context.Set<RefreshToken>();
        }

        public async Task<long> SaveRefreshToken(RefreshToken refreshToken)
        {
            await AddAsync(refreshToken);
            var response = await SaveChangesAsync();

            //if (response < 1)
            //{
            //    throw new ApiException(ApiErrorCodes.InternalError, "unable to Save RefreshToken");
            //}
            return response;
        }

        public async Task RevokeUserTokensAsync(long userId)
        {
            var tokens = await _refreshToken
                .Where(t => t.ACCOUNTID == userId && t.ISREVOKED != 0 && t.EXPIRYDATE > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in tokens)
                token.ISREVOKED = 1;

            await SaveChangesAsync();
        }
        public async Task<RefreshToken> GetByTokenAsync(string refreshToken)
        {
            var refreshTokenInDb = await _refreshToken.Where(e => e.TOKEN == refreshToken).FirstOrDefaultAsync();
            //if (refreshTokenInDb == null)
            //{
            //    throw new ApiException(ApiErrorCodes.BadRequest, "Refresh token does not exists.");
            //}
            return refreshTokenInDb;
        }

    }
}
