using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace FaysalFunds.Persistence.Repositories
{
    public class UserPasswordHistoryRepository : BaseRepository<UserPasswordHistory>, IUserPasswordHistoryRepository
    {
        private readonly DbSet<UserPasswordHistory> _userPasswordHistorySet;

        public UserPasswordHistoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _userPasswordHistorySet = dbContext.Set<UserPasswordHistory>(); // Initialize DbSet
        }
    }
}
