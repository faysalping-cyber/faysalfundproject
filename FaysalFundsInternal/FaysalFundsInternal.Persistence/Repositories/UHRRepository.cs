using FaysalFundsInternal.Domain.Entities;
using FaysalFundsInternal.Domain.Interfaces;
using FaysalFundsInternal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FaysalFundsInternal.Persistence.Repositories
{
    public class UHRRepository : IUHRRepository
    {
        private readonly DbSet<UHR> _uhrSet;
        private readonly DbSet<UHS> _uhsSet;
        public UHRRepository(ApplicationDbContext dbContext)
        {
            _uhrSet = dbContext.Set<UHR>();
            _uhsSet = dbContext.Set<UHS>();
        }

        public async Task<List<UHR>> CheckBalanceAgainstListOfFolios(List<long> folioList)
        {
            var balance = await _uhrSet
    .Where(e => folioList.Contains(e.FOLIONO) /*&& e.BALANCE_AMOUNT != 0.0m*/)
    .Join(_uhsSet,
          uhr => uhr.FOLIONO,
          uhs => uhs.FOLIONO,
          (uhr, uhs) => new { uhr, uhs }) // anonymous object containing both entities
    .GroupBy(e => new { e.uhr.FOLIONO, e.uhr.FUND_NAME })
    .Select(g => g.OrderByDescending(e => e.uhr.NAV_DATE)
                 .Select(x => new UHR
                 {
                     FOLIONO = x.uhr.FOLIONO,
                     FUND_NAME = x.uhr.FUND_NAME,
                     BALANCE_AMOUNT = x.uhr.BALANCE_AMOUNT,
                     ACCOUNTTYPE = x.uhs.ACCOUNTTYPE // now included
                 })
                 .FirstOrDefault())
    .ToListAsync();

            return balance;
        }

        public async Task<List<UHR>> CheckBalanceAgainstFolioNo(long folioNo)
        {
            var balance = await (from uhr in _uhrSet
                                 join uhs in _uhsSet on uhr.FOLIONO equals uhs.FOLIONO
                                 where uhr.FOLIONO == folioNo /*&& uhr.BALANCE_AMOUNT != 0.0m*/
                                 group new { uhr, uhs } by new { uhr.FOLIONO, uhr.FUND_NAME } into g
                                 select g.OrderByDescending(x => x.uhr.NAV_DATE)
                                         .Select(x => new UHR
                                         {
                                             FOLIONO = x.uhr.FOLIONO,
                                             FUND_NAME = x.uhr.FUND_NAME,
                                             BALANCE_AMOUNT = x.uhr.BALANCE_AMOUNT,
                                             // Assuming you have ACCOUNTTYPE in UHR or want to add it
                                             ACCOUNTTYPE = x.uhs.ACCOUNTTYPE
                                         }).FirstOrDefault())
                    .ToListAsync();

            return balance;

        }
    }
}
