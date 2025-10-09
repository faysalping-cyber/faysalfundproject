using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.TransactionAllowed;
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
    public class KpSlabRepository : BaseRepository<kpSlab>, IKpSlabRepository
    {
        private readonly DbSet<kpSlab> _kpSlab;
        public KpSlabRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _kpSlab = dbContext.Set<kpSlab>();
        }
        public async Task<List<kpSlab>> GetAllKuickPayCharges()
        {
            return await _kpSlab
            .ToListAsync();
            //var result = await _kpSlab.ToListAsync();
            //return result ?? new List<KpSlab>();
        }

    }
}
