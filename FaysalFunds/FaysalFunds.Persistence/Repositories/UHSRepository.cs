using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace FaysalFunds.Persistence.Repositories
{
    public class UHSRepository : IUHSRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string easypaisa = "EasyPaisa";
        private readonly string behtari = "Behtari";
        private readonly string[] UnwantedAccountTypes = { "KP-VPS", "EasyPaisa", "YPAY", "EMLAAK" };

        public UHSRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsCNICExists(string cnic) =>
    await _context.UHS.CountAsync(e => e.CNIC == cnic) > 0;

        public async Task<ApiResponseNoData> IsEmailAndMobileExistsAgainstCNIC(string cnic, string cellNo, string email)
        {
            var count = await _context.UHS.CountAsync(e => e.CNIC == cnic && e.CELLNO == cellNo && e.EMAILADDRESS == email);
            if (count == 0)
            {
                return ApiResponseNoData.FailureResponse("Invalid CNIC, mobile, or email combination.");
            }
            return ApiResponseNoData.SuccessResponse();
        }

        public async Task<bool> HaveEasypaisaOrBehtariAccount(string cnic)
        {
           return await _context.UHS.CountAsync(e => e.CNIC == cnic && (e.ACCOUNTTYPE == easypaisa || e.ACCOUNTTYPE == behtari))>0;

        }
        public async Task<List<long>> GetCustomerAvailableFolio(string phoneNo, string cNIC)
        {
            var folioList = await _context.UHS.Where(e => e.CELLNO == phoneNo &&
                                                                e.CNIC == cNIC &&
                                !UnwantedAccountTypes.Contains(e.ACCOUNTTYPE) &&
            e.FOLIOSTATUS == "Active").Select(e => e.FOLIONO).Distinct().ToListAsync();
            return folioList;
        }
    }
}
