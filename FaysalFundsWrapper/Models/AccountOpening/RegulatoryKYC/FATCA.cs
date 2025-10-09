namespace FaysalFundsWrapper.Models
{
    public class FATCA
    {
        public long? UserId { get; set; }
        public string? CountryOfTaxResidenceElsePakistan { get; set; }
        public int? IsUS_CitizenResidentOrHaveGreenCard { get; set; }
        public int? HaveUS_TelephoneNumber { get; set; }
        public string? US_TaxPayerIdentificationNumber { get; set; }
        public int? InstructionsToTransferFundsToUSA { get; set; }
        public int? HaveUS_ResidenceMailingOrHoldingAddress { get; set; }
        public int? IsUs_Born { get; set; }
        public int? FATCA_Declaration { get; set; }
        public int? NonUS_PersonDeclaration { get; set; }
        public string? W9FormUpload { get; set; }
    }
}
