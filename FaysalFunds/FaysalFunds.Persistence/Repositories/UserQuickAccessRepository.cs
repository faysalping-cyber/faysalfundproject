using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Persistence.Repositories
{
    public class UserQuickAccessRepository : BaseRepository<UserQuickAccess>, IUserQuickAccessRepository
    {
        private readonly DbSet<UserQuickAccess> _userquickAccessmenuSet;
        public UserQuickAccessRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _userquickAccessmenuSet = dbContext.Set<UserQuickAccess>();
        }


        //count get
        public async Task<long> GetCountByUserId(long userId)
        {
            return await _userquickAccessmenuSet
                .CountAsync(x => x.USERID == userId);
        }

        //if check validate existing count
        public async Task<UserQuickAccess?> GetExisting(long userId, long quickAccessId)
        {
            return await _userquickAccessmenuSet
                .FirstOrDefaultAsync(x => x.USERID == userId && x.QUICKACCESSID == quickAccessId);
        }

        //Add record
        public async Task<bool> Add(UserQuickAccess entity)
        {
            _userquickAccessmenuSet.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        //Delete

        public async Task<bool> Delete(long userId, long quickAccessId)
        {
            var entity = await GetExisting(userId, quickAccessId);
            if (entity == null) return false;

            _userquickAccessmenuSet.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        //combined record
        public async Task<List<long>> GetQuickAccessIdsByUserId(long userId)
        {
            return await _userquickAccessmenuSet
                .Where(x => x.USERID == userId)
                .Select(x => x.QUICKACCESSID)
                .ToListAsync();
        }


    }
}
