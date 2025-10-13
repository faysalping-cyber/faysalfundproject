using FaysalFunds.Domain.Entities;
using FaysalFunds.Application.DTOs.ExternalAPI;
namespace FaysalFunds.Domain.Interfaces
{
    public interface IFamlFundByFAMLRepository : IBaseRepository<FAML_FUND>
    {
        Task<string?> GetFundNameByFUndShortName(string fundShortName);
        Task<List<RaastIdsModel>> GetRaastIdsModels(List<RaastIdsListResponseModel> listOfIbans);
    }
}