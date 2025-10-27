using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities.TransactionAllowed
{
    [Table("TRANSACTION_RECEIPT_DETAIL")]
    public class TransactionReceiptDetails
    {

        public long ID { get; set; }
        public DateTime DATETIME { get; set; }
        public int FOLIONUMBER { get; set; }
        public string? TRANSACTIONTYPE { get; set; }
        public long? PAYMENTMODE { get; set; }
        public string? FUNDNAME { get; set; }
        public decimal FELCHARGES { get; set; }
        public int? KUICKPAYCHARGES { get; set; }
        public int AMOUNTINVESTED { get; set; }
        public string? MONTHLYPROFIT { get; set; }
        //[Column(TypeName = "decimal(18,3)")]
        public decimal TOTALAMOUNT { get; set; }
        //[MaxLength(100)]
        public string? KUICKPAYID { get; set; }
        public DateTime CREATEDON { get; set; }

        // IBFT-Specific Fields
        public string? IBAN { get; set; }
        public string? BANK_NAME { get; set; }
        public int? IS_EXISTING_ACCOUNT { get; set; } // not bool?
        public byte[]? TRANSACTION_PROOF_PATH { get; set; }
        public int? ACKNOWLEDGE { get; set; }
        public long? FUNDID { get; set; }
        public long? ACCOUNTID { get; set; }
        
        //for conversion and redemption
        public int? STATUS { get; set; }
        public int AVAIL_BALANCE_AT_TRANSACTION { get; set; }
        public string? REJECTI_ON_REASON { get; set; }
        public long? OLD_FUND_ID { get; set; }
        public long? NEW_FUND_ID { get; set; }
        public int? CONVERSION_AMOUNT { get; set; }

        public  int REDEMPTION_AMOUNT { get;set; }
        public int? REDEMPTION_FUND_ID { get; set; }
        public long? REDEMPTION_BANK_ID { get; set; }
        public string? CGT {  get; set; }

    }
}


