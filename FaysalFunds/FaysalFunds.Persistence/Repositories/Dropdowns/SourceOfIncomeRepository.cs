using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.Dropdowns;
using FaysalFunds.Domain.Interfaces.Dropdowns;
using FaysalFunds.Persistence.Database;

namespace FaysalFunds.Persistence.Repositories.Dropdowns
{
    public class SourceOfIncomeRepository : BaseRepository<SourceOfIncome>, ISourceOfIncomeRepository
    {
        public SourceOfIncomeRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<SourceOfIncome>> GetAll()
        {
            return await base.GetAllAsync();
        }
    }
}
