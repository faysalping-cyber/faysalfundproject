using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.RiskProfileDropdowns;
using FaysalFunds.Domain.Interfaces.Dropdowns;
using FaysalFunds.Persistence.Database;

namespace FaysalFunds.Persistence.Repositories.Dropdowns
{
    public class InvestmentObjectivesRepository : BaseRepository<InvestmentObjective>, IInvestmentObjectivesRepository
    {
        public InvestmentObjectivesRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<InvestmentObjective>> GetAll()
        {
            return await base.GetAllAsync();
        }
    }
}
