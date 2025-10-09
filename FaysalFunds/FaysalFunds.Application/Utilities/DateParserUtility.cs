using FaysalFunds.Common;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace FaysalFunds.Application.Utilities
{
    public class DateParserUtility
    {
        private readonly string _dateFormat;

        public DateParserUtility(IOptions<Settings> options)
        {
            _dateFormat = options.Value.DateFormat;
        }

        public DateTime ParseDate(string input)
        {
            return DateTime.ParseExact(input, _dateFormat, CultureInfo.InvariantCulture);
        }

    }
}
