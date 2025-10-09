using FaysalFunds.Domain.Entities.RiskProfileDropdowns;
using FaysalFunds.Domain.Interfaces.Dropdowns;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace FaysalFunds.Persistence.Repositories.Dropdowns
{
    public class AgeRepository : BaseRepository<Age>, IAgeRepository
    {
        public AgeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<IEnumerable<Age>> GetAll()
        {

            return await base.GetAllAsync();
        }
    }
}
