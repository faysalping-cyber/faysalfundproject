using FaysalFunds.Domain.Entities.Dropdowns;
using FaysalFunds.Domain.Interfaces.Dropdowns;
using FaysalFunds.Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Persistence.Repositories.Dropdowns
{
    public class ExpectedNoInvestmentRepository : BaseRepository<ExpectedNoOfInvestmentTransaction>, IExpectedNoInvestmentRepository
    {
        public ExpectedNoInvestmentRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ExpectedNoOfInvestmentTransaction>> GetAll()
        {

            return await base.GetAllAsync();
        }
    }
}
