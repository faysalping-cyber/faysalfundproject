using FaysalFunds.Application.DTOs.ExternalAPI;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace FaysalFunds.Persistence.Repositories
{
    public class FamlFundByFAMLRepository:BaseRepository<FAML_FUND>, IFamlFundByFAMLRepository
    {
        private readonly DbSet<FAML_FUND> _famlFundSet;

        public FamlFundByFAMLRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _famlFundSet = dbContext.FAML_FUND;
        }
        public async Task<List<RaastIdsModel>> GetRaastIdsModels(List<RaastIdsListResponseModel> listOfIbans)
        {
            var fundShortNames = listOfIbans
        .Select(x => x.FundName)
        .Distinct()
        .ToList();
            var filteredFunds = await _famlFundSet
                .Where(f => fundShortNames.Contains(f.FUND_SHORT_NAME))
                .AsNoTracking()
                .ToListAsync();

            var result = from ibanList in listOfIbans
                         join fundList in filteredFunds
                         on ibanList.FundName equals fundList.FUND_SHORT_NAME
                         select new RaastIdsModel
                         {
                             FundName = fundList.FUND_NAME,
                             RaastId = ibanList.IBAN
                         };

            return result.ToList();

        }

        public async Task<string?> GetFundNameByFUndShortName(string fundShortName)
        {
            var fundName = await _famlFundSet.Where(r => r.FUND_SHORT_NAME == fundShortName).Select(e=>e.FUND_NAME).FirstOrDefaultAsync();
            return fundName;
        }
    }
    
}
