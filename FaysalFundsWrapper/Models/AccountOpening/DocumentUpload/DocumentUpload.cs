namespace FaysalFundsWrapper.Models.AccountOpening
{
    public class DocumentUpload
    {
        public long? UserId { get; set; }
        public byte[]? CnicUploadFront { get; set; }
        public byte[]? CnicUploadBack { get; set; }
        public byte[]? LivePhotoOrSelfie { get; set; }
        public byte[]? ProofOfSourceIncome { get; set; }
        public byte[]? PastOneYearBankstatement { get; set; }
        public byte[]? FamilayMember { get; set; }
        public byte[]? InternationalNumber { get; set; }
        public byte[]? CompanyRegister { get; set; }
    }
  
}
