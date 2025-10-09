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
    public class QuickAccessRepository : BaseRepository<QuickAccess>, IQuickAccessRepository
    {
        private readonly DbSet<QuickAccess> _quickAccessmenuSet;
        public QuickAccessRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _quickAccessmenuSet = dbContext.Set<QuickAccess>();
        }

        public async Task<bool> AddQuickAccessMenu(QuickAccess menu)
        {
            await AddAsync(menu);
            return await SaveChangesAsync() > 0;
        }

        public async Task<List<QuickAccess>> GetAllQuickAccessMenu()
        {
            // Only return unselected items (ISSELECTED = 0)
            return await _quickAccessmenuSet
                .Where(m => m.ACTIVE == 1)
                .ToListAsync();
        }

    }
}
