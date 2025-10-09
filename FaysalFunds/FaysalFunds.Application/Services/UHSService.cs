using FaysalFunds.Application.DTOs;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.Enums;
using FaysalFunds.Domain.Interfaces;

namespace FaysalFunds.Application.Services
{
    public class UHSService
    {
        private readonly IUHSRepository _uHSRepository;

        public UHSService(IUHSRepository uHSRepository)
        {
            _uHSRepository = uHSRepository;
        }
        public async Task CheckCustomer(CheckCustomerRequestModel checkCustomer)
        {
            await HaveEasypaisaOrBehtariAccount(checkCustomer.CNIC);
            await IsCNICExists(checkCustomer.CNIC);
        }

        public async Task IsCNICExists(string cnic)
        {
            if (await _uHSRepository.IsCNICExists(cnic))
                throw new ApiException("Your account exists in our records; however, your registered Email Address / Mobile Number may be different than what you have entered. Please try again or reach out to our Customer Services at (021)111-329-725 for assistance", "Account Exists",ErrorIconEnums.AccountExists);
        }

        public async Task HaveEasypaisaOrBehtariAccount(string cnic)
        {
            var isExists =await _uHSRepository.HaveEasypaisaOrBehtariAccount(cnic);
            if (isExists)
                throw new ApiException("We are unable to proceed further since your account is already active with one of our Digital Partners.", "Sorry!",ErrorIconEnums.Sorry);
        }

        public async Task<ApiResponseWithData<AvailableCustomerFolioResponseModel>> GetCustomerAvailableFolio(string phoneNo, string cnic)
        {
            AvailableCustomerFolioResponseModel responseModel = new AvailableCustomerFolioResponseModel();
            var ListOfFolios = await _uHSRepository.GetCustomerAvailableFolio(phoneNo, cnic);
            if (ListOfFolios.Count < 1)
                throw new ApiException("No folio is found.");
            responseModel.FolioList = ListOfFolios;
            return ApiResponseWithData<AvailableCustomerFolioResponseModel>.SuccessResponse(responseModel);
        }
    }
}
