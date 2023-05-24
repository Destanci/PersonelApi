namespace PersonelApi.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetDifferenceInYears(this DateTime startDate, DateTime endDate)
        {
            int years = endDate.Year - startDate.Year;

            if (startDate.Month == endDate.Month &&
                endDate.Day < startDate.Day
                || endDate.Month < startDate.Month)
            {
                years--;
            }

            return years;
        }
    }
}
