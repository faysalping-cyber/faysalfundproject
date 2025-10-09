using FaysalFunds.Domain.Entities;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IFamlFundRepository
    {
        Task<List<FamlFund>> GetActiveFunds();
        Task<bool> IsFundExistsAgainstId(int id);
        Task<FamlFund> GetByActiveFundId(long AccountTypeID);

    }
}
