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
    public class InvestmentFundRepository : BaseRepository<InvestmentFunds>, InvesmentFundRepository
    {
        private readonly DbSet<InvestmentFunds> _investmentfunds;
        public InvestmentFundRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _investmentfunds = dbContext.Set<InvestmentFunds>();
        }
        public async Task<List<InvestmentFunds>> GetAllFunds()
        {
            // Only return unselected items (ISSELECTED = 0)
            return await _investmentfunds
                .Where(i => i.ISENABLE == "1")
                .ToListAsync();
        }
        public async Task<InvestmentFunds> GetByIdAsync(long fundId)
        {
            return await _investmentfunds
                .Where(f => f.ID == fundId)
                .FirstOrDefaultAsync();
        }
    }
    }
