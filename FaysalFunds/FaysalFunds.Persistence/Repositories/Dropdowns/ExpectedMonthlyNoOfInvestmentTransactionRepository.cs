using FaysalFunds.Domain.Entities.Dropdowns;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Persistence.Repositories
{
    public class ExpectedMonthlyNoOfInvestmentTransactionRepository : BaseRepository<ExpectedMonthlyNoOfInvestmentTransaction>, IExpectedMonthlyNoOfInvestmentTransactionRepository
    {
        public ExpectedMonthlyNoOfInvestmentTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ExpectedMonthlyNoOfInvestmentTransaction>> GetAll()
        {
            return await base.GetAllAsync();
        }
    }
}
