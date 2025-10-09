namespace FaysalFundsWrapper.Models.Dashboard
{
    public class TransactionFeaturesDTO
    {
        public long ID { get; set; }

        public string FEATURE_NAME { get; set; }

        public string FEATURE_GROUP { get; set; }
        public string PAYMENT_MODE { get; set; }
        public byte[]? ICON { get; set; } // Base64 icon

    }
    public class TransactionFeaturesGroupedDTO
    {
        public List<TransactionFeaturesDTO> Investment { get; set; }
        public List<TransactionFeaturesDTO> Conversion { get; set; }
        public List<TransactionFeaturesDTO> Withdrawal { get; set; }
    }

    public class TransactionID
    {
        public long TransactionFeatureID { get; set; }
        public long? UserId { get; set; }


    }
}
