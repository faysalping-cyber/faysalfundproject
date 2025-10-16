using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.TransactionAllowed;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Persistence.Repositories
{
    public class TransactionReceiptDetailRepository : BaseRepository<TransactionReceiptDetails>, ITransactionReceiptDetailRepository
    {

        private readonly DbSet<TransactionReceiptDetails> _transactionreceiptDetail;
        public TransactionReceiptDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _transactionreceiptDetail = dbContext.Set<TransactionReceiptDetails>();
        }

        //public async Task<bool> AddReceiptDetail(TransactionReceiptDetails entity)
        //{
        //    // Step 1: normal save (all columns except BLOB)
        //    _transactionreceiptDetail.Add(entity);
        //    await _context.SaveChangesAsync();

        //    // Step 2: Agar proof null nahi hai to BLOB update karo
        //    if (entity.TRANSACTION_PROOF_PATH != null && entity.TRANSACTION_PROOF_PATH.Length > 0)
        //    {
        //        using (var connection = _context.Database.GetDbConnection())
        //        {
        //            if (connection.State != ConnectionState.Open)
        //                await connection.OpenAsync();

        //            using (var command = connection.CreateCommand())
        //            {
        //                command.CommandText = @"
        //            UPDATE TRANSACTION_RECEIPT_DETAILS 
        //            SET TRANSACTION_PROOF_PATH = :p_proof 
        //            WHERE ID = :p_id";

        //                var proofParam = new OracleParameter(":p_proof", OracleDbType.Blob)
        //                {
        //                    Value = entity.TRANSACTION_PROOF_PATH,
        //                    Direction = ParameterDirection.Input
        //                };
        //                command.Parameters.Add(proofParam);

        //                command.Parameters.Add(new OracleParameter(":p_id", OracleDbType.Int64)
        //                {
        //                    Value = entity.ID,
        //                    Direction = ParameterDirection.Input
        //                });

        //                await command.ExecuteNonQueryAsync();
        //            }
        //        }
        //    }

        //    return true;
        //}
        public async Task<bool> SaveKuickPayReceipt(TransactionReceiptDetails entity)
        {
            // Step 1: EF Core se non-BLOB fields save/update
            AttachAndMarkModified(entity,
                x => x.FOLIONUMBER,
                x => x.FUNDNAME,
                x => x.KUICKPAYCHARGES,
                x => x.FELCHARGES,
                x => x.TOTALAMOUNT,
                x => x.MONTHLYPROFIT,
                x => x.AMOUNTINVESTED,
                x => x.KUICKPAYID,
                x => x.PAYMENTMODE,
                x => x.TRANSACTIONTYPE,
                x => x.ACKNOWLEDGE,
                x =>x.ACCOUNTID,
                x => x.FUNDID,
                x => x.DATETIME,
                x => x.CREATEDON
            );

            var efResult = await SaveChangesAsync();

            // KuickPay case me TRANSACTION_PROOF_PATH save/update nahi hota
            return efResult > 0;
        }
        public async Task<bool> SaveIBFTReceipt(TransactionReceiptDetails entity)
        {
            // Step 1: EF Core se non-BLOB fields save/update
            AttachAndMarkModified(entity,
                x => x.FOLIONUMBER,
                x => x.FUNDNAME,
                x => x.FELCHARGES,
                x => x.TOTALAMOUNT,
                x => x.MONTHLYPROFIT,
                x => x.AMOUNTINVESTED,
                x => x.PAYMENTMODE,
                x => x.TRANSACTIONTYPE,
                x => x.BANK_NAME,
                x => x.IBAN,
                x => x.IS_EXISTING_ACCOUNT,
                x => x.ACKNOWLEDGE,
                x => x.ACCOUNTID,
                x => x.FUNDID,
                x => x.DATETIME,
                x => x.CREATEDON
            );

            var efResult = await SaveChangesAsync();

            // Step 2: Sirf IBFT me BLOB update karna hai
            using var connection = _context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"
UPDATE TRANSACTION_RECEIPT_DETAIL SET 
    TRANSACTION_PROOF_PATH = :p_proof
WHERE ID = :p_id";

            var proofParam = new OracleParameter(":p_proof", OracleDbType.Blob)
            {
                Value = entity.TRANSACTION_PROOF_PATH ?? (object)DBNull.Value,
                Direction = ParameterDirection.Input
            };
            command.Parameters.Add(proofParam);

            command.Parameters.Add(new OracleParameter(":p_id", OracleDbType.Int64)
            {
                Value = entity.ID,
                Direction = ParameterDirection.Input
            });

            var rowsAffected = await command.ExecuteNonQueryAsync();

            return efResult + rowsAffected > 0;
        }

        public async Task<List<TransactionReceiptDetails>> GetByFolio(int FolioNumber)
        {
         
            var result = await _transactionreceiptDetail
            .Where(f => f.FOLIONUMBER == FolioNumber)
            .ToListAsync();

            return result ?? new List<TransactionReceiptDetails>();
        }

        public async Task<List<TransactionReceiptDetails>> GetByAccountID(long AccountID)
        {

            var result = await _transactionreceiptDetail
            .Where(f => f.ACCOUNTID == AccountID)
            .ToListAsync();

            return result ?? new List<TransactionReceiptDetails>();
        }

        private void AttachAndMarkModified(TransactionReceiptDetails model, params Expression<Func<TransactionReceiptDetails, object>>[] properties)
        {
            var stub = new TransactionReceiptDetails { ID = model.ID };
            _context.Attach(stub);

            foreach (var propExpr in properties)
            {
                var propertyName = ((MemberExpression)(propExpr.Body is UnaryExpression unary ? unary.Operand : propExpr.Body)).Member.Name;
                var value = typeof(TransactionReceiptDetails).GetProperty(propertyName)?.GetValue(model);
                typeof(TransactionReceiptDetails).GetProperty(propertyName)?.SetValue(stub, value);
                _context.Entry(stub).Property(propertyName).IsModified = true;
            }
        }
    }
}
