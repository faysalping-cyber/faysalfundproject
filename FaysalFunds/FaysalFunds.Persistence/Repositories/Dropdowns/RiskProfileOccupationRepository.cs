using FaysalFunds.Domain.Entities.Dropdowns;
using FaysalFunds.Domain.Entities.RiskProfileDropdowns;
using FaysalFunds.Domain.Interfaces.Dropdowns;
using FaysalFunds.Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Persistence.Repositories.Dropdowns
{
    public class RiskProfileOccupationRepository : BaseRepository<RiskProfileOccupation>, IRiskProfileOccupationRepository
    {
        public RiskProfileOccupationRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<RiskProfileOccupation>> GetAll()
        {
            return await base.GetAllAsync();
        }
    }
}
