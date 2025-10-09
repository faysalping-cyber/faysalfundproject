using FaysalFunds.Domain.Entities;


namespace FaysalFunds.Domain.Interfaces
{
    public interface ITransactionPinRepository
    {
        Task<bool> GenerateTransactionPin(TransactionPin model);

        Task<TransactionPin> GetTpinByAccountOpeningId(long accountOpeningId);
        Task<bool> UpdateTransactionPin(TransactionPin model);


    }
}
