using FaysalFunds.Common;
using FaysalFunds.Common.ApiResponses;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IUHSRepository
    {
        Task<List<long>> GetCustomerAvailableFolio(string phoneNo, string cNIC);
        Task<bool> HaveEasypaisaOrBehtariAccount(string cnic);
        Task<bool> IsCNICExists(string cnic);
        Task<ApiResponseNoData> IsEmailAndMobileExistsAgainstCNIC(string cnic, string cellNo, string email);

    }
}
