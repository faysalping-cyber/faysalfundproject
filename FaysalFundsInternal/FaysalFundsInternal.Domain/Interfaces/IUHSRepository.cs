
namespace FaysalFundsInternal.Domain.Interfaces
{
    public interface IUHSRepository
    {
        Task<bool> DoesFolioExistAgainstGivenCNICAndPhoneNo(string cNIC, string phoneNo, long folioNo);
        Task<int> FilterRecords(long folio);
        Task<List<long>> GetAvailableCustomerFolios(string cnic, string cellNo);
        Task<List<long>> GetCustomerAvailableFolioForMobileApp(string phoneNo, string cNIC);
        Task<List<long>> GetFolioByCNICAndPhoneNo(string phoneNo, string cNIC);
    }
}
