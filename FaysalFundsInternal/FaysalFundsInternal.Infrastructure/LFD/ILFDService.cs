using FaysalFundsInternal.CrossCutting.Responses;
using FaysalFundsInternal.Infrastructure.DTOs;

namespace FaysalFundsInternal.Infrastructure
{
    public interface ILFDService
    {
        Task<ApiResponse<SearchResponseModel>> Search(string cnic);
    }
}
