using FaysalFundsInternal.Domain.Entities;
using FaysalFundsInternal.Domain.Model;

namespace FaysalFundsInternal.Domain.Interfaces
{
    public interface IAccountStatementRepository
    {
        Task<List<ProfitModel>> FiscalProfit(long folioNo);
        Task<List<FamlAccountSatement>> GetAccountSatement(string unitHolderId, string fundId);
        Task<List<ProfitModel>> ProfitSinceInception(long folioNo);
    }
}
