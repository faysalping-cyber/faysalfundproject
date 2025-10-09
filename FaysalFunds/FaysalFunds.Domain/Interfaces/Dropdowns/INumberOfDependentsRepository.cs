using FaysalFunds.Domain.Entities.RiskProfileDropdowns;

namespace FaysalFunds.Domain.Interfaces.Dropdowns
{
    public interface INumberOfDependentsRepository
    {
        Task<IEnumerable<NoOfDependents>> GetAll();
    }
}
