using FaysalFundsInternal.Domain.Model;

namespace FaysalFundsInternal.Application.DTOs
{
    public class MobileAppProfitModel
    {
        public long Folio { get; set; }
        public List<ProfitModel> FundProfit { get; set; }
    }
}
