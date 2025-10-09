using FaysalFunds.Domain.Entities.Dropdowns;
using FaysalFunds.Domain.Interfaces.Dropdowns;
using FaysalFunds.Persistence.Database;

namespace FaysalFunds.Persistence.Repositories.Dropdowns
{
    public class BankRepository : BaseRepository<Bank>, IBankRepository
    {
        public BankRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Bank>> GetAll()
        {
            return await base.GetAllAsync();
        }
    }
}
