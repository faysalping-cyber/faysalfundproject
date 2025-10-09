namespace FaysalFundsInternal.Domain.Interfaces
{
    public interface IFamlFundDetailRepository
    {
        decimal CalculateFiscalNav(string fundId);
        decimal GetClosingNav(string fundId);
    }
}
