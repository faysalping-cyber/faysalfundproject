namespace FaysalFundsWrapper.Models.AccountOpening
{
    public class UploadDigitalDocuments
    {
        public long? UserId { get; set; }
        public long AccountTypeID { get; set; }
        public string? CnicUploadFront { get; set; }
        public string? CnicUploadBack { get; set; }
        public string? LivePhotoOrSelfie { get; set; }


    }
}
