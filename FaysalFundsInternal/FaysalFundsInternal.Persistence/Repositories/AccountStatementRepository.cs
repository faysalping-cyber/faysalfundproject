using FaysalFundsInternal.Application.DTOs;
using FaysalFundsInternal.CrossCutting;
using FaysalFundsInternal.Domain.Entities;
using FaysalFundsInternal.Domain.Interfaces;
using FaysalFundsInternal.Domain.Model;
using FaysalFundsInternal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FaysalFundsInternal.Persistence.Repositories
{

    public class AccountStatementRepository : BaseRepository<FamlAccountSatement>, IAccountStatementRepository
    {
        private readonly IFamlFundDetailRepository _famlFundDetailRepository;
        private readonly IUnitHolderRepository _unitHolderRepository;
        private readonly DbSet<FamlAccountSatement> _accountStatementSet;
        private readonly DbSet<FamlFund> _famlFunds;
        public AccountStatementRepository(ApplicationDbContext dbContext, IUnitHolderRepository unitHolderRepository, IFamlFundDetailRepository famlFundDetailRepository) : base(dbContext)
        {
            _accountStatementSet = dbContext.Set<FamlAccountSatement>();
            _unitHolderRepository = unitHolderRepository;
            _famlFundDetailRepository = famlFundDetailRepository;
            _famlFunds = dbContext.Set<FamlFund>();
        }
        
        public async Task<List<FamlAccountSatement>> GetAccountSatement(string unitHolderId, string fundId)
        {
            var accountStatements = await _dbSet.Where(account => account.UNIT_HOLDER_ID == unitHolderId && account.FUND_ID == fundId)
                .ToListAsync();
            return accountStatements;
        }

        private async Task<decimal> SumInGrossAmount(long folioNo, string fundId)
        {

            var unitHolderId = await _unitHolderRepository.GetUnitHolderId(folioNo);
            var accountStatements =await GetAccountSatement(unitHolderId, fundId);
            // Step 2: Perform in-memory calculations
            var amountIn = accountStatements.Sum(ac =>
            {
                switch (ac.STAMPING_FIELD)
                {
                    case "Investment":
                    case "Transfer In-GFT":
                    case "Online Sale":
                    case "Sale of Unit Instruction":
                    case "Contribution":
                    case "Redemption of Unit Instruction":
                    case "Online Investment":
                    case "Transfer In":
                    case "Transfer In-TDI":
                    case "Investment Adjustment":
                        Console.WriteLine(ac.STAMPING_FIELD + ": " + ac.GROSS_AMOUNT);
                        return ac.GROSS_AMOUNT;
                    default:
                        return 0;
                }
            });
            return amountIn;
        }
        private async Task<decimal> SumOutGrossAmount(long folioNo, string fundId)
        {
            var unitHolderId = await _unitHolderRepository.GetUnitHolderId(folioNo);
            var accountStatements =await GetAccountSatement(unitHolderId, fundId);
            var amountOut = accountStatements.Sum(ac =>
            {
                switch (ac.STAMPING_FIELD)
                {
                    case "Maturity":
                    case "Transfer Out":
                    case "Investment Reversal":
                    case "Online Redemption":
                    case "Transfer Out-GFT":
                    case "Retirement":
                    case "Transfer In-PFT":
                    case "Redemption":
                    case "Transfer Out-PFT":
                    case "Transfer Out-TDI":
                        Console.WriteLine(ac.STAMPING_FIELD + ": " + ac.GROSS_AMOUNT);
                        return ac.GROSS_AMOUNT;
                    default:
                        return 0;
                }
            });

            return amountOut;
        }
        private async Task<List<FundBalanceModel>> GetClosingBalance(long folioNo)
        {
            // Get the Unit Holder ID based on the folio number
            var unitHolderId =await _unitHolderRepository.GetUnitHolderId(folioNo);

            // Fetch the relevant data from the database
            var balanceData =await (from account in _dbSet
                               where account.UNIT_HOLDER_ID == unitHolderId
                                     && account.NAV_DATE <= DateTime.Now
                               group account by account.FUND_ID into grouped
                               select new
                               {
                                   FundId = grouped.Key,
                                   TotalUnits = grouped.Sum(e => e.NO_OF_UNITS)
                               }).ToListAsync();

            // Create a list of FundBalanceModel using a foreach loop for async operations
            var balance = new List<FundBalanceModel>();
            foreach (var e in balanceData)
            {
                var closingNav = _famlFundDetailRepository.GetClosingNav(e.FundId); // Await for each NAV calculation
                balance.Add(new FundBalanceModel
                {
                    FundId = e.FundId,
                    Balance = e.TotalUnits * closingNav
                });
            }

            return balance;
        }
        private async Task<List<FundBalanceModel>> FiscalOpeningBalance(long folioNo)
        {
            // Get the Unit Holder ID based on the folio number
            var unitHolderId = await _unitHolderRepository.GetUnitHolderId(folioNo);

            // Get the fiscal date (assuming it can be executed outside the query)
            var fiscalDate = FiscalDate.GetFiscalDate();

            // Fetch the relevant data from the database
            var balanceData = await (from account in _dbSet
                               where account.UNIT_HOLDER_ID == unitHolderId
                                     && account.NAV_DATE <= fiscalDate
                               group account by account.FUND_ID into grouped
                               select new
                               {
                                   FundId = grouped.Key,
                                   TotalUnits = grouped.Sum(g => g.NO_OF_UNITS)
                               }).ToListAsync();
            // Calculate the latest fiscal price for each fund and compute the balance in-memory
            var balance = balanceData.Select(e => new FundBalanceModel
            {
                FundId = e.FundId,
                Balance = e.TotalUnits * _famlFundDetailRepository.CalculateFiscalNav(e.FundId) // Apply method in-memory
            }).ToList();

            return balance;
        }

        public async Task<List<ProfitModel>> FiscalProfit(long folioNo)
        {
            var closingBalance = await GetClosingBalance(folioNo);
            var fiscalOpeningBalance = await FiscalOpeningBalance(folioNo);

            var rawFundBalance = (from c in closingBalance
                                  join f in fiscalOpeningBalance on c.FundId equals f.FundId
                                  join pro in _famlFunds on f.FundId equals pro.FUND_ID
                                  select new
                                  {
                                      FundName = pro.FUND_NAME,
                                      c.FundId,
                                      CurrentBalance = c.Balance,
                                      FiscalBalance = f.Balance
                                  }).ToList();

            // Run all async operations in parallel
            var fundBalance = new List<ProfitModel>();

            foreach (var item in rawFundBalance)
            {
                var outGross = await SumOutGrossAmount(folioNo, item.FundId);
                var inGross = await SumInGrossAmount(folioNo, item.FundId);

                fundBalance.Add(new ProfitModel
                {
                    Fund = item.FundName,
                    Profit = (outGross + item.CurrentBalance) - (inGross + item.FiscalBalance)
                });
            }

            return fundBalance;

        }
        public async Task<List<ProfitModel>> ProfitSinceInception(long folioNo)
        {
            var closingBalance = await GetClosingBalance(folioNo);

            var rawFundBalance = (from c in closingBalance
                                  join pro in _famlFunds on c.FundId equals pro.FUND_ID.ToString()
                                  select new
                                  {
                                      FundName = pro.FUND_NAME,
                                      c.FundId,
                                      CurrentBalance = c.Balance,
                                  }).ToList();

            var fundBalance = new List<ProfitModel>();

            foreach (var item in rawFundBalance)
            {
                var sumOut = await SumOutGrossAmount(folioNo, item.FundId);
                var sumIn = await SumInGrossAmount(folioNo, item.FundId);

                fundBalance.Add(new ProfitModel
                {
                    Fund = item.FundName,
                    Profit = (sumOut + item.CurrentBalance) - sumIn
                });
            }

            return fundBalance;
        }

    }
}