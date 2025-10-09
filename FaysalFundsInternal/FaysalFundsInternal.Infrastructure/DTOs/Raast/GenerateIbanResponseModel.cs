namespace FaysalFundsInternal.Infrastructure.DTOs.Raast
{
    public class GenerateIbanResponseModel
    {
        public string errorRemarks { get; set; } = string.Empty;
        public string raastIBAN { get; set; } = string.Empty;
        public string responseCode { get; set; } = string.Empty;
        public string responseStatus { get; set; } = string.Empty;
    }
}
