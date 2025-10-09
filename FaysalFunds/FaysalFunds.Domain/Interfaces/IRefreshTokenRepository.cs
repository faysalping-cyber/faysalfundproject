using FaysalFunds.Domain.Entities;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByTokenAsync(string refreshToken);
        Task RevokeUserTokensAsync(long userId);
        public Task<long> SaveRefreshToken(RefreshToken refreshToken);
    }
}
