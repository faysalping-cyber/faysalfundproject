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
    public class ExpectedMonthlyNoOfRedemptionTransactionRepository : BaseRepository<ExpectedMonthlyNoOfRedemptionTransaction>, IExpectedMonthlyNoOfRedemptionTransactionRepository
    {

        public ExpectedMonthlyNoOfRedemptionTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ExpectedMonthlyNoOfRedemptionTransaction>> GetAll()
        {
            return await base.GetAllAsync();
        }
    }
}
