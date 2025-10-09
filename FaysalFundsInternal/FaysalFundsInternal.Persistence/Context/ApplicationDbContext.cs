using FaysalFundsInternal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FaysalFundsInternal.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<UnitHolder> UnitHolder { get; set; }
        public DbSet<FamlFundDetail> FamlFundDetail { get; set; }
        public DbSet<FamlAccountSatement> FamlAccountSatement { get; set; }
        public DbSet<FamlFund> FamlFund { get; set; }
        // Views
        public DbSet<UHR> UHR { get; set; }
        public DbSet<UHS> UHS { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UHR>().ToView("UHR");
            modelBuilder.Entity<UHS>().ToView("UHS");

        }
    }
}
