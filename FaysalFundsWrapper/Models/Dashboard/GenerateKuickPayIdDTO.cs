namespace FaysalFundsWrapper.Models.Dashboard
{
    public class GenerateKuickPayIdDTO
    {
        public string FolioNo { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public long? UserId { get; set; }

    }

    public class KuickPayResponseModel
    {
        public string response_Code { get; set; }
        public string response_description { get; set; }
        public string responseCode_Detail { get; set; }
    }
    public class KuickPayFinalResponse
    {
        public string ResponseCode { get; set; }
        public string KuickPayId { get; set; }   // 👈 required name
        public string ResponseDetail { get; set; }
    }

}
