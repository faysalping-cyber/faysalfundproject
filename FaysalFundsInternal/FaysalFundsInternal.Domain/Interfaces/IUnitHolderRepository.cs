namespace FaysalFundsInternal.Domain.Interfaces
{
    public interface IUnitHolderRepository
    {
        Task<string> GetUnitHolderId(long folioNo);
    }
}
