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
        public string TRANSACTIONTYPE { get; set; }
        public long PAYMENTMODE { get; set; }
        public string FUNDNAME { get; set; }
        public int FELCHARGES { get; set; }
        public int? KUICKPAYCHARGES { get; set; }
        public int AMOUNTINVESTED { get; set; }
        public string MONTHLYPROFIT { get; set; }
        //[Column(TypeName = "decimal(18,3)")]
        public decimal TOTALAMOUNT { get; set; }
        //[MaxLength(100)]
        public string? KUICKPAYID { get; set; }
        public DateTime CREATEDON { get; set; }

        // IBFT-Specific Fields
        public string? IBAN { get; set; }
        public string? BANK_NAME { get; set; }
        public string? ACCOUNT_TITLE { get; set; }
        public int? IS_EXISTING_ACCOUNT { get; set; } // not bool?
        public byte[]? TRANSACTION_PROOF_PATH { get; set; }
        public int? ACKNOWLEDGE { get; set; }



    }
}


