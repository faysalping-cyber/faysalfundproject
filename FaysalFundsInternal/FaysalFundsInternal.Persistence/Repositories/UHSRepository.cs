using FaysalFundsInternal.CrossCutting.Constants;
using FaysalFundsInternal.Domain.Entities;
using FaysalFundsInternal.Domain.Interfaces;
using FaysalFundsInternal.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FaysalFundsInternal.Persistence.Repositories
{
    public class UHSRepository : IUHSRepository
    {
        private readonly DbSet<UHS> _uhsSet;
        private readonly BlockedAccountTypes _blockedAccountTypes;
        private readonly HashSet<string> specialTypes;
        private readonly string[] UnwantedAccountTypesForMobileApp = { "KP-VPS", "EasyPaisa", "YPAY", "EMLAAK" };
        public UHSRepository(ApplicationDbContext dbContext, IOptions<BlockedAccountTypes> blockedAccountTypes)
        {
            _uhsSet = dbContext.Set<UHS>();
            _blockedAccountTypes = blockedAccountTypes.Value;
            specialTypes = new HashSet<string> { _blockedAccountTypes.KPVPS, _blockedAccountTypes.EasyPaisa, _blockedAccountTypes.YPAY };
        }
        public async Task<List<long>> GetFolioByCNICAndPhoneNo(string phoneNo, string cNIC)
        {
            var folioList = await _uhsSet.Where(e => e.CELLNO == phoneNo && e.CNIC == cNIC).Select(e => e.FOLIONO).ToListAsync();
            return folioList;
        }
        public async Task<List<long>> GetAvailableCustomerFolios(string cnic, string cellNo)
        {
            var response = await _uhsSet
                .Where(e => e.CELLNO == cellNo && e.CNIC == cnic).Select(e => new { e.FOLIONO, e.ACCOUNTTYPE, e.FOLIOSTATUS })
                .ToListAsync();
            
            var filteredRecords = response
                .Where(record =>
                         (!specialTypes.Contains(record.ACCOUNTTYPE) && record.FOLIOSTATUS != "Blocked"))
                .Select(r => r.FOLIONO)
                .Distinct()
                .ToList();
            return filteredRecords;
        }
        public async Task<bool> DoesFolioExistAgainstGivenCNICAndPhoneNo(string cNIC, string phoneNo, long folioNo)
        {
            var FolioList = await GetFolioByCNICAndPhoneNo(phoneNo, cNIC);
            bool isExist = FolioList.Contains(folioNo);
            return isExist;
        }
        public async Task<int> FilterRecords(long folio)
        {
            var count =await _uhsSet
                            .Where(record =>
                                     (!specialTypes.Contains(record.ACCOUNTTYPE) && record.FOLIOSTATUS != "Blocked" && record.FOLIONO == folio))
                            .Distinct()
                            .CountAsync();
            return count;
        }

        public async Task<List<long>> GetCustomerAvailableFolioForMobileApp(string phoneNo, string cNIC)
        {
            var folioList = await _uhsSet.Where(e => e.CELLNO == phoneNo &&
                                                                e.CNIC == cNIC &&
                                !UnwantedAccountTypesForMobileApp.Contains(e.ACCOUNTTYPE) &&
            e.FOLIOSTATUS == "Active").Select(e => e.FOLIONO).Distinct().ToListAsync();
            return folioList;
        }
    }
}