namespace FaysalFundsWrapper.Models
{


  
    public class GenerateTPinRequest
    {
        public long? AccountOpeningId { get; set; }

        public string? Pin { get; set; }
       
    }
    public class VerifyTpinRequest
    {
        public long? AccountOpeningId { get; set; }

        public string? Pin { get; set; }
    }
    public class ChangeTPinRequest
    {
        public long AccountOpeningId { get; set; }
        public string OldPin { get; set; }
        public string NewPin { get; set; }
    }
    public class ForgotTPinRequest
    {
        public long AccountOpeningId { get; set; }
        public string NewPin { get; set; }
    }
}
