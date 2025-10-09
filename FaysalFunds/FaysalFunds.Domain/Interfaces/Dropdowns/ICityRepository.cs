using FaysalFunds.Domain.Entities.Dropdowns;

namespace FaysalFunds.Domain.Interfaces.Dropdowns
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAll();
    }
}
