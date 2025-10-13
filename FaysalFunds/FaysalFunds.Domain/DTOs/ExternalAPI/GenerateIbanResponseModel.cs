namespace FaysalFunds.Application.DTOs.ExternalAPI
{
    public class GenerateIbanResponseModel
    {
        public string FolioNo { get; set; } = string.Empty;
        public string FundName { get; set; } = string.Empty;
        public string RaastIban { get; set; } = string.Empty;
    }
}
