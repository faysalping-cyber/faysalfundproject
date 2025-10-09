namespace FaysalFundsWrapper.Models
{
    public class CRS
    {
        public long? UserId { get; set; }
        public int? TaxResidentCountry { get; set; }
        public string? TIN_Number { get; set; }
        public int? HaveTIN { get; set; }
        public int? ReasonForNoTIN { get; set; }
        public int? CRS_Declaration { get; set; }
    }
}