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
    public class InvestmentInstructionRepository : BaseRepository<TransactionReceiptDetails>, IInvestmentInstructionRepository
    {
        private readonly DbSet<InvestmentInstructions> _investmentMethids;
        public InvestmentInstructionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _investmentMethids = dbContext.Set<InvestmentInstructions>();
        }
        public async Task<List<InvestmentInstructions>> GetInvestmentMethods()
        {
            // Only return unselected items (ISSELECTED = 0)
            return await _investmentMethids
                .ToListAsync();
        }
    }
}
