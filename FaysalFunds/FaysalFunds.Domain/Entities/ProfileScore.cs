using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFunds.Domain.Entities
{
    [Table("RISKPROFILE_SCORE")]
    public class ProfileScore
    {
        public long ID { get; set; }
        [ForeignKey(nameof(AccountOpening))]
        public long ACCOUNTOPENING_ID { get; set; }
        public int TOTAL_SCORE { get; set; }
        public string RISKLEVEL { get; set; }

        //Navigational Property
        public AccountOpening AccountOpening { get; set; }
    }
}