namespace FaysalFundsInternal.CrossCutting
{
    public static class FiscalDate
    {
        public static DateTime GetFiscalDate()
        {
            DateTime currentDate = DateTime.Now;
            DateTime june30;

            if (currentDate.Month == 6 && currentDate.Day == 30)
            {
                june30 = new DateTime(currentDate.Year - 1, 6, 30);
            }
            else
            {
                june30 = new DateTime(currentDate.Year, 6, 30);
            }
            return june30;
        }
    }
}
