namespace FaysalFundsWrapper.Models.Raast
{
    public class GenerateRaastIdRequestModel
    {
        public string FundCode { get; set; } = string.Empty;
        public string FolioNo { get; set; } = string.Empty;
        public string? CNIC { get; set; } = string.Empty;
        public string? CellNo { get; set; } = string.Empty;
    }
}
