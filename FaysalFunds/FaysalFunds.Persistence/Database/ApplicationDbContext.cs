using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.Dropdowns;
using FaysalFunds.Domain.Entities.RiskProfileDropdowns;
using FaysalFunds.Domain.Entities.TransactionAllowed;
using FaysalFunds.Domain.Entities.Views;
using Microsoft.EntityFrameworkCore;

namespace FaysalFunds.Persistence.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<UserPasswordHistory> UserPasswordHistories { get; set; }
        public DbSet<JwtBlacklist> JwtBlacklists { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AccountOpening> AccountOpening { get; set; }
        public DbSet<FamlFund> FamlFunds { get; set; }
        public DbSet<ProfileScore> ProfileScore { get; set; }
        public DbSet<QuickAccess> QuickAccess { get; set; }
        public DbSet<UserQuickAccess> UserQuickAccesses { get; set; }
        public DbSet<TransactionPin> TransactionPin { get; set; }
        public DbSet<InvestmentFunds> InvestmentFunds { get; set; }
        public DbSet<TransactionTypesGroup> TransactionTypesGroup { get; set; }
        public DbSet<TransactionFeatures> TransactionFeatures { get; set; }
        public DbSet<FundTransactionGroup> FundTransactionGroup { get; set; }
        public DbSet<FundFeaturePermission> FundFeaturePermission { get; set; }
        public DbSet<kpSlab> kpSlabs { get; set; }
        public DbSet<TransactionReceiptDetails> TransactionReceiptDetails { get; set; }
        public DbSet<InvestmentInstructions> InvestmentInstructions { get; set; }
        // Views

        public DbSet<UHS> UHS { get; set; }

        //Dropdowns
        public DbSet<Age> Age { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<ExpectedInvesmentAmmount> ExpectedInvesmentAmmount { get; set; }
        public DbSet<ExpectedNoOfRedemptionTransactions> ExpectedNoOfRedemptionTransactions { get; set; }
        public DbSet<FinancialPosition> FinancialPosition { get; set; }
        public DbSet<InvestmentHorizon> InvestmentHorizon { get; set; }
        public DbSet<InvestmentKnowledge> InvestmentKnowledge { get; set; }
        public DbSet<InvestmentObjective> InvestmentObjectives { get; set; }
        public DbSet<MartialStatus> MartialStatus { get; set; }
        public DbSet<MobileOwnership> MobileOwnership { get; set; }
        public DbSet<NoOfDependents> NoOfDependents { get; set; }
        public DbSet<NoTINNumberReason> NoTINNumberReason { get; set; }
        public DbSet<Occupation> Occupation { get; set; }
        public DbSet<Profession> Profession { get; set; }
        public DbSet<RiskAppetite> RiskAppetite { get; set; }
        public DbSet<SourceOfIncome> SourceOfIncome { get; set; }
        public DbSet<WalletName> WalletName { get; set; }
        public DbSet<ExpectedNoOfInvestmentTransaction> ExpectedNoOfInvestmentTransaction { get; set; }
        public DbSet<ExpectedMonthlyInvestment> ExpectedMonthlyInvestment { get; set; }
        public DbSet<ExpectedMonthlyNoOfInvestmentTransaction> ExpectedMonthlyNoOfInvestmentTransaction { get; set; }
        public DbSet<ExpectedMonthlyNoOfRedemptionTransaction> ExpectedMonthlyNoOfRedemptionTransaction { get; set; }


        public DbSet<ExpectedNoOfRedemptionTransactions> ExpectedNoOfRedemptionTransaction { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UHS>().ToView("UHS");
        }
    }
}