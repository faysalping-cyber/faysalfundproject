using FaysalFunds.Domain.Entities.Dropdowns;
using FaysalFunds.Domain.Interfaces.Dropdowns;
using FaysalFunds.Persistence.Database;

namespace FaysalFunds.Persistence.Repositories.Dropdowns
{
    public class CityRepository:BaseRepository<City>,ICityRepository
    {
        public CityRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<City>> GetAll()
        {
            var asd = await base.GetAllAsync();
            return asd;
        }
    }
}
