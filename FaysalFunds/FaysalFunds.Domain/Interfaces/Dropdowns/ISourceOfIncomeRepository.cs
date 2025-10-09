using FaysalFunds.Domain.Entities.Dropdowns;

namespace FaysalFunds.Domain.Interfaces.Dropdowns
{
    public interface ISourceOfIncomeRepository
    {
        Task<IEnumerable<SourceOfIncome>> GetAll();
    }
}
