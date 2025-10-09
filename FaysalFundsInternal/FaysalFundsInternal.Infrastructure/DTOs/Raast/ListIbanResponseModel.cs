namespace FaysalFundsInternal.Infrastructure.DTOs.Raast
{
    public class ListIbanResponseModel
    {
        public List<IbanListModel> IbanList { get; set; }
    }
    public class IbanListModel
    {
        public string FundName { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
    }
}
