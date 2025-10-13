namespace FaysalFundsInternal.Infrastructure.DTOs.Raast
{
    public class GenerateIbanResponseModel
    {
        public string ErrorRemarks { get; set; } = string.Empty;
        public string RaastIBAN { get; set; } = string.Empty;
        public string ResponseCode { get; set; } = string.Empty;
        public string ResponseStatus { get; set; } = string.Empty;
        public string? ResponseMessage { get; set; } = string.Empty;
    }
}
