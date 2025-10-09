using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.RiskProfileDropdowns;
using FaysalFunds.Domain.Interfaces.Dropdowns;
using FaysalFunds.Persistence.Database;

namespace FaysalFunds.Persistence.Repositories.Dropdowns
{
    public class NumberOfDependentsRepository : BaseRepository<NoOfDependents>, INumberOfDependentsRepository
    {
        public NumberOfDependentsRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<NoOfDependents>> GetAll()
        {
            return await base.GetAllAsync();
        }
    }
}
