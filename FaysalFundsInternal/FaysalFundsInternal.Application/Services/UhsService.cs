using FaysalFundsInternal.CrossCutting.Responses;
using FaysalFundsInternal.Domain.Interfaces;

namespace FaysalFundsInternal.Application.Services
{
    public class UhsService
    {
        private readonly IUHSRepository _uhsRepository;
        public UhsService(IUHSRepository uhsRepository)
        {
            _uhsRepository = uhsRepository;
        }
        public async Task DoesFolioExistAgainstGivenCNICAndPhoneNo(string cNIC, string phoneNo, long folioNo)
        {
            var isExists = await _uhsRepository.DoesFolioExistAgainstGivenCNICAndPhoneNo(cNIC, phoneNo, folioNo);
            if (!isExists)
            {
                throw new ApiException("Folio doesn't exists against the cell no. and CNIC.");
            }
        }

        public async Task FilterFolio(long folio)
        {
            int count = await _uhsRepository.FilterRecords(folio);
            if (count<1)
            {
                throw new ApiException("Folio does not found.");
            }
        }

        public async Task<List<long>> GetCustomerAvailableFolioForMobileApp(string cnic , string phoneNo)
        {
            var  folioList  = await _uhsRepository.GetCustomerAvailableFolioForMobileApp(phoneNo, cnic);
            if (folioList.Count<1)
            {
                throw new ApiException("No folio is found");
            }
            return folioList;
        }
    }
}