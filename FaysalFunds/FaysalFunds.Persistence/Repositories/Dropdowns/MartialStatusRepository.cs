using FaysalFunds.Domain.Entities;
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
    public class MartialStatusRepository : BaseRepository<MartialStatus>, IMartialStatusRepository
    {
        public MartialStatusRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<MartialStatus>> GetAll()
        {
            return await base.GetAllAsync();
        }
    }
}
