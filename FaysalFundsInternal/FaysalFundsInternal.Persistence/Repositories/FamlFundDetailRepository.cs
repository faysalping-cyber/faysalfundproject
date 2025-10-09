using FaysalFundsInternal.CrossCutting;
using FaysalFundsInternal.Domain.Entities;
using FaysalFundsInternal.Domain.Interfaces;
using FaysalFundsInternal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FaysalFundsInternal.Persistence.Repositories
{
    public class FamlFundDetailRepository : BaseRepository<FamlFundDetail>, IFamlFundDetailRepository
    {
        private readonly DbSet<FamlFundDetail> _famlFundDetailSet;
        public FamlFundDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _famlFundDetailSet = dbContext.Set<FamlFundDetail>();
        }
        public decimal GetClosingNav(string fundId)
        {
            var currentNav = (from d in _dbSet
                              where d.FUND_ID == fundId && d.APPLICABLE_NAV_DATE == _famlFundDetailSet
                        .Where(fd => fd.FUND_ID == d.FUND_ID && fd.APPLICABLE_NAV_DATE <= DateTime.Now)
                        .Max(fd => fd.APPLICABLE_NAV_DATE)
                              select d.NAV_PER_UNIT).FirstOrDefault();
            return currentNav;
        }

        public decimal CalculateFiscalNav(string fundId)
        {
            var currentNav = (from d in _dbSet
                              where d.FUND_ID == fundId && d.APPLICABLE_NAV_DATE == _context.FamlFundDetail
                        .Where(fd => fd.FUND_ID == d.FUND_ID && fd.APPLICABLE_NAV_DATE <= FiscalDate.GetFiscalDate())
                        .Max(fd => fd.APPLICABLE_NAV_DATE)
                              select d.NAV_PER_UNIT).FirstOrDefault();
            return currentNav;
        }

    }
}
