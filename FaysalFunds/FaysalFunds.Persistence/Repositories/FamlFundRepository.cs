using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace FaysalFunds.Persistence.Repositories
{
    public class FamlFundRepository : BaseRepository<FamlFund>, IFamlFundRepository
    {
        private readonly DbSet<FamlFund> _famlFundSet;
        public FamlFundRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _famlFundSet = dbContext.Set<FamlFund>();
        }
        public async Task<bool> IsFundExistsAgainstId(int id)
        {
           return await _famlFundSet.CountAsync(e=>e.ID == id)>0;
        }

        public async Task<List<FamlFund>> GetActiveFunds()
        {
           var activeFundsList =  await _famlFundSet.Where(e => e.STATUS == 1).ToListAsync();
            return activeFundsList;
        }
        public async Task<FamlFund> GetByActiveFundId(long AccountTypeID)
        {
            return await _famlFundSet
              .Where(f => f.ID == AccountTypeID)
                .FirstOrDefaultAsync();
        }
    }
}