namespace FaysalFundsWrapper.Models.Dashboard
{
    public class FundTransactionGroupDTO
    {

        public long FundId { get; set; }
        public long TransactionTypeId { get; set; }

    }
    public class FeaturePermissionResultDTO
    {
        public string FeatureName { get; set; }
        public string FeatureGroup { get; set; }
        public bool IsAllowed { get; set; }

    }
    public class FeaturePermissionRequestDTO
    {
        public long FundId { get; set; }
        public long TransactionFeatureId { get; set; }
        public long? UserId { get; set; }

    }

}


