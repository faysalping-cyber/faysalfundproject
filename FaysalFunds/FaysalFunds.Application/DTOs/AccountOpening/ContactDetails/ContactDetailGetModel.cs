namespace FaysalFunds.Application.DTOs.AccountOpening
{
    public class ContactDetailGetModel
    {
        public string? PermanentAddress { get; set; }
        public int? Country { get; set; }
        public int? City { get; set; }
        public int MailingAddressSameAsPermanent { get; set; }
        public string? CountryCode { get; set; }
        public string? MobileNumber { get; set; }
        public int? MobileNumberOwnership { get; set; }
        public string? Email { get; set; }
        public string? MalingAddress1 { get; set; }
        public int? MalingCity { get; set; } // dropdown
        public int? MalingCountry { get; set; } // dropdown
    }
}
