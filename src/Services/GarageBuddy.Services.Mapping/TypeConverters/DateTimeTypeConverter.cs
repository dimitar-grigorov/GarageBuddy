namespace GarageBuddy.Services.Mapping.TypeConverters
{
    using System;
    using System.Globalization;

    using AutoMapper;

    using Common.Constants;

    public class DateTimeTypeConverter : ITypeConverter<string, DateTime?>
    {
        public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
        {
            var result = null as DateTime?;

            if (DateTime.TryParseExact(source, GlobalConstants.DefaultDateTimeFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                result = dateTime;
            }

            if (DateTime.TryParseExact(source, GlobalConstants.DefaultDateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOnly))
            {
                result = dateOnly;
            }

            if (result is { Year: < GlobalConstants.MinValidYear })
            {
                result = null;
            }

            return result;
        }
    }
}
