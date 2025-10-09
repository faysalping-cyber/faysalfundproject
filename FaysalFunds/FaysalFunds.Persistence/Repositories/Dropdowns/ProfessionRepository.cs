using FaysalFunds.Domain.Entities;
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
    public class ProfessionRepository : BaseRepository<Profession>, IProfessionRepository
    {
        public ProfessionRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Profession>> GetAll()
        {
            return await base.GetAllAsync();
        }
    }
}
