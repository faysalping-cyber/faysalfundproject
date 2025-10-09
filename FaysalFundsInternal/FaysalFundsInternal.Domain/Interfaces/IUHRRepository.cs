using FaysalFundsInternal.Domain.Entities;

namespace FaysalFundsInternal.Domain.Interfaces
{
    public interface IUHRRepository
    {
        Task<List<UHR>> CheckBalanceAgainstFolioNo(long folioNo);
        Task<List<UHR>> CheckBalanceAgainstListOfFolios(List<long> folioList);
    }
}
