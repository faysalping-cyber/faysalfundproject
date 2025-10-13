namespace FaysalFunds.Application.DTOs.ExternalAPI
{
    public class GenerateIbanRequestModel
    {
        public string FundCode { get; set; } = string.Empty;
        public string FolioNo { get; set; } = string.Empty;
        public string? CNIC { get; set; } = string.Empty;
        public string? CellNo { get; set; } = string.Empty;
    }
}
