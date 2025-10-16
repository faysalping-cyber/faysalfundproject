using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.TransactionAllowed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Interfaces
{
    public interface ITransactionReceiptDetailRepository
    {
        Task<bool> SaveKuickPayReceipt(TransactionReceiptDetails entity);
        Task<bool> SaveIBFTReceipt(TransactionReceiptDetails entity);

        Task<List<TransactionReceiptDetails>> GetByFolio(int FolioNumber);
        Task<List<TransactionReceiptDetails>> GetByAccountID(long AccountID);

    }
}
