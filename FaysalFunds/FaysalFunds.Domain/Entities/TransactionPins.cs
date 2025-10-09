
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FaysalFunds.Domain.Entities
{
    [Table("TRANSACTION_PIN")]

    public class TransactionPin
    {
        public long ID { get; set; }

        [ForeignKey(nameof(AccountOpening))]
        public long ACCOUNTOPENING_ID { get; set; }

        public string? PINHASH { get; set; }
        public int? FAILEDATTEMPTS { get; set; }
        public int? ISLOCKED { get; set; }
        public DateTime? LOCKEDON { get; set; }
        public DateTime? CREATEDON { get; set; }

        public DateTime? UPDATEDON { get; set; }

    }
}





