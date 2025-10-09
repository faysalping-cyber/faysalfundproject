using FaysalFunds.Domain.Entities.RiskProfileDropdowns;
using FaysalFunds.Domain.Interfaces.Dropdowns;
using FaysalFunds.Persistence.Database;

namespace FaysalFunds.Persistence.Repositories.Dropdowns
{
    public class OccupationRepository : BaseRepository<Occupation>, IOccupationRepository
    {
        public OccupationRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Occupation>> GetAll()
        {
            return await base.GetAllAsync();
        }
    }
}
