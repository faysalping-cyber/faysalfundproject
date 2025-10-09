namespace FaysalFundsInternal.Infrastructure.DTOs.Raast
{
    public class GenerateIbanRequestModel
    {
        public string? Channel { get; set; } = string.Empty;
        public string FundCode { get; set; } = string.Empty;
        public string FolioNo { get; set; } = string.Empty;
        public string CNIC { get; set; } = string.Empty;
        public string CellNo { get; set; } = string.Empty;
    }
}